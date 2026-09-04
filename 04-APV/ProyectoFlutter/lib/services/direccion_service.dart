import 'dart:convert';

import 'package:http/http.dart' as http;

import '../config/api_config.dart';
import '../models/direccion_model.dart';
import 'auth_service.dart';

class DireccionService {
  final String _baseUrl = ApiConfig.baseUrl;
  final AuthService _authService = AuthService();

  Future<Map<String, String>> _getHeaders() async {
    final token = await _authService.getToken();
    return {
      'Content-Type': 'application/json',
      if (token != null) 'Authorization': 'Bearer $token',
    };
  }

  Future<List<Direccion>> getDireccionesUsuario(int idUsuario) async {
    try {
      final headers = await _getHeaders();
      final response = await http
          .get(Uri.parse('$_baseUrl/Direcciones'), headers: headers)
          .timeout(const Duration(seconds: 10));

      if (response.statusCode == 200) {
        final List<dynamic> data = jsonDecode(response.body);
        final todasDirecciones = data
            .map((json) => Direccion.fromJson(json))
            .toList();
        return todasDirecciones.where((d) => d.idUsuario == idUsuario).toList();
      } else {
        throw Exception('Error al obtener direcciones: ${response.statusCode}');
      }
    } catch (e) {
      throw Exception('Error de conexión al obtener direcciones: $e');
    }
  }
}
