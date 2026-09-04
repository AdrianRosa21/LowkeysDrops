import 'dart:convert';

import 'package:http/http.dart' as http;

import '../config/api_config.dart';
import '../models/pedido_resumen_model.dart';
import 'auth_service.dart';

class PedidoService {
  final String _baseUrl = ApiConfig.baseUrl;
  final AuthService _authService = AuthService();

  Future<Map<String, String>> _getHeaders() async {
    final token = await _authService.getToken();
    return {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer $token',
    };
  }

  Future<List<PedidoResumen>> getPedidosCliente(int idCliente) async {
    try {
      final headers = await _getHeaders();
      final response = await http
          .get(
            Uri.parse('$_baseUrl/Pedidos/cliente/$idCliente'),
            headers: headers,
          )
          .timeout(const Duration(seconds: 10));

      if (response.statusCode == 200) {
        final List<dynamic> data = jsonDecode(response.body);
        return data.map((json) => PedidoResumen.fromJson(json)).toList();
      } else {
        throw Exception('Error al obtener pedidos: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Error de conexión al obtener pedidos: $e');
    }
  }

  // Create an order and add a product to it
  Future<void> crearPedido(
    int idCliente,
    int idDireccion,
    String metodoPago,
    int idProducto,
    int cantidad,
  ) async {
    try {
      final headers = await _getHeaders();

      // 1. Crear el pedido
      final responsePedido = await http
          .post(
            Uri.parse('$_baseUrl/Pedidos'),
            headers: headers,
            body: jsonEncode({
              'idCliente': idCliente,
              'idDireccion': idDireccion,
              'metodoPago': metodoPago,
            }),
          )
          .timeout(const Duration(seconds: 10));

      if (responsePedido.statusCode == 201) {
        final data = jsonDecode(responsePedido.body);
        final idPedido = data['idPedido'];

        // 2. Agregar producto al pedido
        final responseProducto = await http
            .post(
              Uri.parse('$_baseUrl/Pedidos/$idPedido/productos'),
              headers: headers,
              body: jsonEncode({
                'idProducto': idProducto,
                'cantidad': cantidad,
              }),
            )
            .timeout(const Duration(seconds: 10));

        if (responseProducto.statusCode != 200) {
          throw Exception(
            'Error al agregar el producto al pedido: ${responseProducto.statusCode}',
          );
        }
      } else {
        throw Exception(
          'Error al crear el pedido: ${responsePedido.statusCode} ${responsePedido.body}',
        );
      }
    } catch (e) {
      throw Exception('Error de conexión al crear pedido: $e');
    }
  }

  Future<void> confirmarRecepcion(int idPedido, int idCliente) async {
    try {
      final headers = await _getHeaders();
      final response = await http
          .put(
            Uri.parse('$_baseUrl/Pedidos/$idPedido/confirmar-recepcion'),
            headers: headers,
            body: jsonEncode({'idCliente': idCliente}),
          )
          .timeout(const Duration(seconds: 10));

      if (response.statusCode != 200) {
        throw Exception('Error al confirmar recepción: ${response.statusCode} ${response.body}');
      }
    } catch (e) {
      throw Exception('Error de conexión al confirmar recepción: $e');
    }
  }
}
