import 'package:flutter/material.dart';
import '../models/producto_catalogo_model.dart';
import '../models/direccion_model.dart';
import '../services/direccion_service.dart';
import '../services/pedido_service.dart';
import '../services/auth_service.dart';
import '../config/api_config.dart';

class CheckoutScreen extends StatefulWidget {
  final ProductoCatalogo producto;

  const CheckoutScreen({super.key, required this.producto});

  @override
  State<CheckoutScreen> createState() => _CheckoutScreenState();
}

class _CheckoutScreenState extends State<CheckoutScreen> {
  final _direccionService = DireccionService();
  final _pedidoService = PedidoService();
  final _authService = AuthService();

  List<Direccion> _direcciones = [];
  Direccion? _direccionSeleccionada;
  String _metodoPago = 'CONTRA_ENTREGA'; // Metodo por defecto
  
  bool _isLoading = true;
  String? _errorMessage;
  int? _idCliente;

  @override
  void initState() {
    super.initState();
    _loadData();
  }

  Future<void> _loadData() async {
    setState(() {
      _isLoading = true;
      _errorMessage = null;
    });

    try {
      final user = await _authService.getCurrentUser();
      if (user == null) {
        setState(() {
          _errorMessage = 'Debes iniciar sesión para comprar.';
          _isLoading = false;
        });
        return;
      }
      
      _idCliente = user.idUsuario;
      final direcciones = await _direccionService.getDireccionesUsuario(user.idUsuario);
      
      setState(() {
        _direcciones = direcciones;
        if (direcciones.isNotEmpty) {
          _direccionSeleccionada = direcciones.first;
        }
        _isLoading = false;
      });
    } catch (e) {
      setState(() {
        _errorMessage = 'No se pudieron cargar las direcciones.';
        _isLoading = false;
      });
    }
  }

  Future<void> _crearPedido() async {
    if (_direccionSeleccionada == null || _idCliente == null) return;

    setState(() {
      _isLoading = true;
    });

    try {
      await _pedidoService.crearPedido(
        _idCliente!,
        _direccionSeleccionada!.idDireccion,
        _metodoPago,
        widget.producto.idProducto,
        1,
      );

      if (!mounted) return;
      
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(content: Text('¡Pedido creado exitosamente!'), backgroundColor: Colors.green),
      );
      
      // Regresar al catálogo o pantalla anterior
      Navigator.pop(context);
    } catch (e) {
      setState(() {
        _isLoading = false;
      });
      if (!mounted) return;
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error al crear pedido: $e'), backgroundColor: Colors.red),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    final String resolvedImage = ApiConfig.resolveImage(widget.producto.imagenUrl);

    return Scaffold(
      appBar: AppBar(
        title: const Text('CONFIRMAR COMPRA'),
      ),
      body: _isLoading
          ? const Center(child: CircularProgressIndicator())
          : _errorMessage != null
              ? Center(child: Text(_errorMessage!, style: const TextStyle(color: Colors.red)))
              : _buildCheckoutForm(resolvedImage),
    );
  }

  Widget _buildCheckoutForm(String resolvedImage) {
    return SingleChildScrollView(
      padding: const EdgeInsets.all(24.0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const Text(
            'Resumen del producto',
            style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
          ),
          const SizedBox(height: 16),
          Card(
            child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: Row(
                children: [
                  Container(
                    width: 80,
                    height: 80,
                    decoration: BoxDecoration(
                      color: Colors.white10,
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: resolvedImage.isNotEmpty
                        ? Image.network(
                            resolvedImage,
                            fit: BoxFit.cover,
                            errorBuilder: (context, error, stackTrace) =>
                                const Icon(Icons.broken_image, color: Colors.grey),
                          )
                        : const Icon(Icons.image, color: Colors.grey),
                  ),
                  const SizedBox(width: 16),
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          widget.producto.nombre,
                          style: const TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
                        ),
                        const SizedBox(height: 4),
                        Text(
                          '\$${widget.producto.precio.toStringAsFixed(2)}',
                          style: const TextStyle(color: Colors.white70),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ),
          ),
          const SizedBox(height: 32),
          const Text(
            'Dirección de envío',
            style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
          ),
          const SizedBox(height: 16),
          if (_direcciones.isEmpty)
            const Text(
              'No tienes direcciones registradas. Debes crear una desde la web primero.',
              style: TextStyle(color: Colors.red),
            )
          else
            ..._direcciones.map((direccion) {
              return RadioListTile<Direccion>(
                contentPadding: EdgeInsets.zero,
                title: Text('${direccion.tipo} - ${direccion.municipio}'),
                subtitle: Text(direccion.direccionTexto),
                value: direccion,
                groupValue: _direccionSeleccionada,
                onChanged: (value) {
                  setState(() {
                    _direccionSeleccionada = value;
                  });
                },
              );
            }),
            
          const SizedBox(height: 32),
          const Text(
            'Método de pago',
            style: TextStyle(fontSize: 20, fontWeight: FontWeight.bold),
          ),
          const SizedBox(height: 16),
          DropdownButtonFormField<String>(
            initialValue: _metodoPago,
            decoration: InputDecoration(
              border: OutlineInputBorder(borderRadius: BorderRadius.circular(8)),
              filled: true,
              fillColor: Colors.white10,
            ),
            items: const [
              DropdownMenuItem(value: 'CONTRA_ENTREGA', child: Text('CONTRA ENTREGA')),
              DropdownMenuItem(value: 'TRANSFERENCIA', child: Text('TRANSFERENCIA')),
              DropdownMenuItem(value: 'DEPOSITO', child: Text('DEPÓSITO')),
            ],
            onChanged: (value) {
              if (value != null) {
                setState(() {
                  _metodoPago = value;
                });
              }
            },
          ),
          
          const SizedBox(height: 40),
          SizedBox(
            width: double.infinity,
            height: 55,
            child: ElevatedButton(
              style: ElevatedButton.styleFrom(
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
              ),
              onPressed: _direcciones.isEmpty ? null : _crearPedido,
              child: const Text('CONFIRMAR Y CREAR PEDIDO', style: TextStyle(fontSize: 16)),
            ),
          ),
        ],
      ),
    );
  }
}
