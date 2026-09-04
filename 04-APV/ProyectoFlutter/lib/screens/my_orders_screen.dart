import 'package:flutter/material.dart';

import '../models/pedido_resumen_model.dart';
import '../services/pedido_service.dart';
import '../services/auth_service.dart';

class MyOrdersScreen extends StatefulWidget {
  const MyOrdersScreen({super.key});

  @override
  State<MyOrdersScreen> createState() => _MyOrdersScreenState();
}

class _MyOrdersScreenState extends State<MyOrdersScreen> {
  final _pedidoService = PedidoService();
  final _authService = AuthService();

  List<PedidoResumen> _pedidos = [];
  bool _isLoading = true;
  String? _errorMessage;

  @override
  void initState() {
    super.initState();
    _fetchPedidos();
  }

  Future<void> _fetchPedidos() async {
    setState(() {
      _isLoading = true;
      _errorMessage = null;
    });

    try {
      final user = await _authService.getCurrentUser();
      if (user == null) throw Exception('Usuario no autenticado');

      final pedidos = await _pedidoService.getPedidosCliente(user.idUsuario);

      setState(() {
        _pedidos = pedidos;
        // order by most recent (assuming higher ID or just parse date, let's reverse for visual effect)
        _pedidos = _pedidos.reversed.toList();
        _isLoading = false;
      });
    } catch (e) {
      setState(() {
        _errorMessage = 'No se pudieron cargar los pedidos.';
        _isLoading = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: const Text('MIS PEDIDOS')),
      body: _buildBody(),
    );
  }

  Widget _buildBody() {
    if (_isLoading) {
      return const Center(child: CircularProgressIndicator());
    }

    if (_errorMessage != null) {
      return Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Text(_errorMessage!, style: const TextStyle(color: Colors.red)),
            const SizedBox(height: 16),
            ElevatedButton(
              onPressed: _fetchPedidos,
              child: const Text('REINTENTAR'),
            ),
          ],
        ),
      );
    }

    if (_pedidos.isEmpty) {
      return const Center(child: Text('Aún no tienes pedidos.'));
    }

    return RefreshIndicator(
      onRefresh: _fetchPedidos,
      child: ListView.builder(
        padding: const EdgeInsets.all(16),
        itemCount: _pedidos.length,
        itemBuilder: (context, index) {
          final pedido = _pedidos[index];
          return Card(
            margin: const EdgeInsets.only(bottom: 16),
            child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Text(
                        'Pedido #${pedido.idPedido}',
                        style: const TextStyle(
                          fontWeight: FontWeight.bold,
                          fontSize: 18,
                        ),
                      ),
                      Text(
                        pedido.estadoPedido,
                        style: TextStyle(
                          color: _getColorForStatus(pedido.estadoPedido),
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                    ],
                  ),
                  const Divider(height: 24),
                  Text('Fecha: ${pedido.fechaPedido.split('T')[0]}'),
                  Text('Total: \$${pedido.total.toStringAsFixed(2)}'),
                  const SizedBox(height: 8),
                  Text(
                    'Dirección: ${pedido.tipoDireccion} - ${pedido.municipio}',
                    style: const TextStyle(color: Colors.grey),
                  ),
                  if (pedido.estadoPedido.toUpperCase() == 'PENDIENTE_CONFIRMACION') ...[
                    const SizedBox(height: 16),
                    SizedBox(
                      width: double.infinity,
                      child: ElevatedButton(
                        style: ElevatedButton.styleFrom(
                          backgroundColor: Colors.green,
                          foregroundColor: Colors.white,
                        ),
                        onPressed: () async {
                          try {
                            final user = await _authService.getCurrentUser();
                            if (user != null) {
                              await _pedidoService.confirmarRecepcion(pedido.idPedido, user.idUsuario);
                              if (context.mounted) {
                                ScaffoldMessenger.of(context).showSnackBar(
                                  const SnackBar(content: Text('Recepción confirmada exitosamente.')),
                                );
                                _fetchPedidos();
                              }
                            }
                          } catch (e) {
                            if (context.mounted) {
                              ScaffoldMessenger.of(context).showSnackBar(
                                SnackBar(content: Text('Error: $e'), backgroundColor: Colors.red),
                              );
                            }
                          }
                        },
                        child: const Text('CONFIRMAR RECEPCIÓN'),
                      ),
                    ),
                  ],
                ],
              ),
            ),
          );
        },
      ),
    );
  }

  Color _getColorForStatus(String status) {
    switch (status.toUpperCase()) {
      case 'PENDIENTE':
        return Colors.orange;
      case 'EN CAMINO':
      case 'EN_CAMINO':
        return Colors.blue;
      case 'ENTREGADO':
        return Colors.green;
      case 'CANCELADO':
        return Colors.red;
      case 'PENDIENTE_CONFIRMACION':
        return Colors.lightBlue;
      default:
        return Colors.white;
    }
  }
}
