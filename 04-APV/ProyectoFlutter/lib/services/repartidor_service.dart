import 'dart:convert';
import 'package:http/http.dart' as http;
import '../config/api_config.dart';
import '../models/pedido_resumen_model.dart';
import 'auth_service.dart';

class RepartidorService {
  final String _baseUrl = ApiConfig.baseUrl;
  final AuthService _authService = AuthService();

  Future<Map<String, String>> _getHeaders() async {
    final token = await _authService.getToken();
    return {
      'Content-Type': 'application/json',
      'Authorization': 'Bearer $token',
    };
  }

  Future<List<PedidoResumen>> getPedidosDisponibles() async {
    final headers = await _getHeaders();
    final response = await http.get(
      Uri.parse('$_baseUrl/Repartidor/pedidos-disponibles'),
      headers: headers,
    );

    if (response.statusCode == 200) {
      final List<dynamic> data = jsonDecode(response.body);
      return data.map((json) => PedidoResumen.fromJson(json)).toList();
    } else {
      throw Exception('Error al obtener pedidos disponibles');
    }
  }

  Future<List<PedidoResumen>> getHistorialPedidos(int idRepartidor) async {
    final headers = await _getHeaders();
    final response = await http.get(
      Uri.parse('$_baseUrl/Repartidor/pedidos/$idRepartidor'),
      headers: headers,
    );

    if (response.statusCode == 200) {
      final List<dynamic> data = jsonDecode(response.body);
      return data.map((json) => PedidoResumen.fromJson(json)).toList();
    } else {
      throw Exception('Error al obtener historial de pedidos');
    }
  }

  Future<void> tomarPedido(int idPedido, int idRepartidor) async {
    final headers = await _getHeaders();
    final response = await http.post(
      Uri.parse('$_baseUrl/Pedidos/$idPedido/tomar'),
      headers: headers,
      body: jsonEncode({'idRepartidor': idRepartidor}),
    );
    if (response.statusCode != 200) throw Exception('No se pudo tomar el pedido');
  }

  Future<void> marcarEnCamino(int idPedido, int idRepartidor) async {
    final headers = await _getHeaders();
    final response = await http.put(
      Uri.parse('$_baseUrl/Pedidos/$idPedido/en-camino'),
      headers: headers,
      body: jsonEncode({'idRepartidor': idRepartidor}),
    );
    if (response.statusCode != 200) throw Exception('No se pudo marcar en camino');
  }

  Future<void> registrarEntrega(int idPedido, int idRepartidor, String fotoUrl, String observacion) async {
    final headers = await _getHeaders();
    final response = await http.put(
      Uri.parse('$_baseUrl/Pedidos/$idPedido/entrega'),
      headers: headers,
      body: jsonEncode({
        'idRepartidor': idRepartidor,
        'fotoEntregaUrl': fotoUrl,
        'observacion': observacion,
      }),
    );
    if (response.statusCode != 200) throw Exception('No se pudo registrar la entrega');
  }

  Future<void> registrarEntregaFallida(int idPedido, int idRepartidor, String observacion) async {
    final headers = await _getHeaders();
    final response = await http.put(
      Uri.parse('$_baseUrl/Pedidos/$idPedido/entrega-fallida'),
      headers: headers,
      body: jsonEncode({
        'idRepartidor': idRepartidor,
        'observacion': observacion,
      }),
    );
    if (response.statusCode != 200) throw Exception('No se pudo registrar la entrega fallida');
  }
}
