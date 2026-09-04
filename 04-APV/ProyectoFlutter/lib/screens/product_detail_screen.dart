import 'package:flutter/material.dart';
import '../models/producto_catalogo_model.dart';
import '../config/api_config.dart';
import '../services/auth_service.dart';
import 'checkout_screen.dart';
import 'login_screen.dart';

class ProductDetailScreen extends StatelessWidget {
  final ProductoCatalogo producto;

  const ProductDetailScreen({super.key, required this.producto});

  Future<void> _handleBuyClick(BuildContext context) async {
    final authService = AuthService();
    final token = await authService.getToken();

    if (!context.mounted) return;

    if (token == null) {
      // Must login to buy
      Navigator.push(
        context,
        MaterialPageRoute(builder: (_) => const LoginScreen()),
      ).then((_) async {
        // After login screen pops, check if we are logged in now
        if (await authService.getToken() != null && context.mounted) {
          Navigator.push(
            context,
            MaterialPageRoute(builder: (_) => CheckoutScreen(producto: producto)),
          );
        }
      });
    } else {
      Navigator.push(
        context,
        MaterialPageRoute(builder: (_) => CheckoutScreen(producto: producto)),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    final bool disponible = producto.estado == 'DISPONIBLE' && producto.stock > 0;
    final String resolvedImage = ApiConfig.resolveImage(producto.imagenUrl);

    return Scaffold(
      appBar: AppBar(
        title: Text(producto.nombre),
      ),
      body: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            SizedBox(
              height: 400,
              child: resolvedImage.isNotEmpty
                  ? Image.network(
                      resolvedImage,
                      fit: BoxFit.cover,
                      errorBuilder: (context, error, stackTrace) =>
                          const Center(child: Icon(Icons.broken_image, size: 100, color: Colors.grey)),
                    )
                  : const Center(
                      child: Icon(Icons.image, size: 100, color: Colors.grey),
                    ),
            ),
            Padding(
              padding: const EdgeInsets.all(24.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: [
                      Text(
                        '\$${producto.precio.toStringAsFixed(2)}',
                        style: const TextStyle(
                          fontSize: 32,
                          fontWeight: FontWeight.bold,
                        ),
                      ),
                      Container(
                        padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
                        decoration: BoxDecoration(
                          color: disponible ? Colors.green.withValues(alpha: 0.2) : Colors.red.withValues(alpha: 0.2),
                          borderRadius: BorderRadius.circular(20),
                          border: Border.all(
                            color: disponible ? Colors.green : Colors.red,
                            width: 2,
                          ),
                        ),
                        child: Text(
                          producto.estado,
                          style: TextStyle(
                            color: disponible ? Colors.green : Colors.red,
                            fontWeight: FontWeight.bold,
                            letterSpacing: 1.2,
                          ),
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: 16),
                  Text(
                    producto.nombre,
                    style: const TextStyle(
                      fontSize: 24,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 8),
                  Text(
                    'Categoría: ${producto.categoria} | Drop: ${producto.dropNombre}',
                    style: const TextStyle(
                      color: Colors.grey,
                      fontSize: 16,
                    ),
                  ),
                  if (producto.talla != null && producto.talla!.isNotEmpty) ...[
                    const SizedBox(height: 12),
                    Text(
                      'Talla: ${producto.talla}',
                      style: const TextStyle(
                        fontSize: 18,
                        fontWeight: FontWeight.w500,
                      ),
                    ),
                  ],
                  const SizedBox(height: 32),
                  const Text(
                    'Descripción',
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 12),
                  Text(
                    producto.descripcion ?? 'Sin descripción.',
                    style: const TextStyle(
                      fontSize: 16,
                      height: 1.6,
                      color: Colors.white70,
                    ),
                  ),
                  const SizedBox(height: 48),
                  SizedBox(
                    width: double.infinity,
                    height: 60,
                    child: ElevatedButton(
                      style: ElevatedButton.styleFrom(
                        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(8)),
                      ),
                      onPressed: disponible ? () => _handleBuyClick(context) : null,
                      child: const Text('COMPRAR AHORA', style: TextStyle(fontSize: 18)),
                    ),
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
