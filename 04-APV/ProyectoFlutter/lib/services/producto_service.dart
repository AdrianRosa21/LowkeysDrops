import 'dart:convert';

import 'package:http/http.dart' as http;

import '../config/api_config.dart';
import '../models/producto_catalogo_model.dart';

class ProductoService {
  final String _baseUrl = ApiConfig.baseUrl;

  Future<List<ProductoCatalogo>> getCatalogo() async {
    try {
      final response = await http
          .get(Uri.parse('$_baseUrl/Catalogo'))
          .timeout(const Duration(seconds: 10));

      if (response.statusCode == 200) {
        final List<dynamic> data = jsonDecode(response.body);
        return data.map((json) => ProductoCatalogo.fromJson(json)).toList();
      } else {
        throw Exception('Error al obtener el catálogo: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Error de conexión al obtener catálogo: $e');
    }
  }

  Future<ProductoCatalogo> getProductoById(int id) async {
    // Note: there is no /api/Catalogo/{id} so we might need to get all and filter
    // or use /api/Productos/{id}. The web app likely uses /api/Productos/{id} or gets it from catalog.
    // Let's use /api/Productos/{id} and manually map, or just get from the catalog list directly.
    try {
      final response = await http
          .get(Uri.parse('$_baseUrl/Productos/$id'))
          .timeout(const Duration(seconds: 10));
      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        // Map ProductoResponseDto to ProductoCatalogo for simplicity in the UI
        return ProductoCatalogo(
          idProducto: data['idProducto'],
          nombre: data['nombre'],
          descripcion: data['descripcion'],
          talla: data['talla'],
          precio: (data['precio'] as num).toDouble(),
          imagenUrl: data['imagenUrl'],
          esUnico: data['esUnico'],
          stock: data['stock'],
          estado: data['estado'],
          categoria: data['idCategoria']
              .toString(), // It returns idCategoria, not name
          dropNombre: '', // We don't have this in ProductoResponseDto
          cantidadResenas: 0,
        );
      } else {
        throw Exception('Error al obtener el producto: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Error de conexión al obtener producto: $e');
    }
  }
}
