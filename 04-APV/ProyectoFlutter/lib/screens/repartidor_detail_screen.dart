import 'package:flutter/material.dart';
import '../models/pedido_resumen_model.dart';
import '../services/repartidor_service.dart';

class RepartidorDetailScreen extends StatefulWidget {
  final PedidoResumen pedido;
  final int repartidorId;

  const RepartidorDetailScreen({super.key, required this.pedido, required this.repartidorId});

  @override
  State<RepartidorDetailScreen> createState() => _RepartidorDetailScreenState();
}

class _RepartidorDetailScreenState extends State<RepartidorDetailScreen> {
  final _repartidorService = RepartidorService();
  bool _isLoading = false;

  Future<void> _actionWrapper(Future<void> Function() action, String successMessage) async {
    setState(() => _isLoading = true);
    try {
      await action();
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text(successMessage), backgroundColor: Colors.green));
      Navigator.pop(context);
    } catch (e) {
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(SnackBar(content: Text('Error: $e'), backgroundColor: Colors.red));
    } finally {
      if (mounted) setState(() => _isLoading = false);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text('Pedido #${widget.pedido.idPedido}')),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(24.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text('Estado actual: ${widget.pedido.estadoPedido}', style: const TextStyle(fontSize: 18, fontWeight: FontWeight.bold)),
            const SizedBox(height: 24),
            _buildDetail('Cliente', widget.pedido.cliente),
            _buildDetail('Teléfono', widget.pedido.telefono),
            _buildDetail('Dirección', '${widget.pedido.municipio} - ${widget.pedido.direccionTexto}'),
            _buildDetail('Total a cobrar', '\$${widget.pedido.total.toStringAsFixed(2)}'),
            const SizedBox(height: 40),
            if (_isLoading)
              const Center(child: CircularProgressIndicator())
            else
              ..._buildActionButtons(),
          ],
        ),
      ),
    );
  }

  Widget _buildDetail(String title, String value) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 16.0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(title, style: const TextStyle(color: Colors.grey)),
          const SizedBox(height: 4),
          Text(value, style: const TextStyle(fontSize: 16)),
        ],
      ),
    );
  }

  List<Widget> _buildActionButtons() {
    final estado = widget.pedido.estadoPedido.toUpperCase();
    
    // Si no tiene entregas asignadas (asumiendo PENDIENTE o PREPARADO)
    if (widget.pedido.idRepartidor == null) {
      return [
        _buildButton('TOMAR PEDIDO', Colors.blue, () {
          _actionWrapper(() => _repartidorService.tomarPedido(widget.pedido.idPedido, widget.repartidorId), 'Pedido tomado');
        }),
      ];
    }
    
    // Si ya lo tiene este repartidor asignado, mostramos botones de estado
    if (widget.pedido.idRepartidor == widget.repartidorId) {
      if (estado == 'EN CAMINO' || estado == 'EN_CAMINO') {
        return [
          _buildButton('MARCAR COMO ENTREGADO', Colors.green, () {
            _mostrarDialogoEntrega();
          }),
          const SizedBox(height: 16),
          _buildButton('MARCAR ENTREGA FALLIDA', Colors.red, () {
            _actionWrapper(() => _repartidorService.registrarEntregaFallida(widget.pedido.idPedido, widget.repartidorId, "Cliente ausente"), 'Entrega fallida registrada');
          }),
        ];
      } else if (estado != 'ENTREGADO' && estado != 'CANCELADO' && estado != 'FALLIDO' && estado != 'PENDIENTE_CONFIRMACION') {
         // Puede marcarlo en camino
         return [
           _buildButton('MARCAR EN CAMINO', Colors.orange, () {
              _actionWrapper(() => _repartidorService.marcarEnCamino(widget.pedido.idPedido, widget.repartidorId), 'Pedido en camino');
           }),
         ];
      }
    }
    return [];
  }

  Widget _buildButton(String text, Color color, VoidCallback onPressed) {
    return SizedBox(
      width: double.infinity,
      height: 50,
      child: ElevatedButton(
        style: ElevatedButton.styleFrom(backgroundColor: color, foregroundColor: Colors.white),
        onPressed: onPressed,
        child: Text(text),
      ),
    );
  }

  void _mostrarDialogoEntrega() {
    final urlController = TextEditingController();
    final obsController = TextEditingController();

    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text('Registrar Entrega'),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              TextField(
                controller: urlController,
                decoration: const InputDecoration(labelText: 'URL Foto de Entrega (obligatorio)', border: OutlineInputBorder()),
              ),
              const SizedBox(height: 16),
              TextField(
                controller: obsController,
                decoration: const InputDecoration(labelText: 'Observación (opcional)', border: OutlineInputBorder()),
              ),
            ],
          ),
          actions: [
            TextButton(onPressed: () => Navigator.pop(context), child: const Text('CANCELAR')),
            ElevatedButton(
              onPressed: () {
                if (urlController.text.trim().isEmpty) return;
                Navigator.pop(context);
                _actionWrapper(() => _repartidorService.registrarEntrega(widget.pedido.idPedido, widget.repartidorId, urlController.text.trim(), obsController.text.trim()), 'Entrega registrada');
              },
              child: const Text('CONFIRMAR'),
            ),
          ],
        );
      },
    );
  }
}
