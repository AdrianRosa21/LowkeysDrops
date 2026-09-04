import 'dart:convert';

import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';

import '../config/api_config.dart';
import '../models/auth_response_model.dart';
import '../models/usuario_model.dart';

class AuthService {
  final String _baseUrl = ApiConfig.baseUrl;

  Future<AuthResponse> login(String correo, String contrasena) async {
    final response = await http.post(
      Uri.parse('$_baseUrl/auth/login'),
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode({'correo': correo, 'contrasena': contrasena}),
    );

    if (response.statusCode == 200) {
      final data = jsonDecode(response.body);
      final authResponse = AuthResponse.fromJson(data);
      await _saveSession(authResponse);
      return authResponse;
    } else {
      throw Exception(
        'Error de inicio de sesión: ${response.statusCode} - ${response.body}',
      );
    }
  }

  Future<void> _saveSession(AuthResponse authResponse) async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.setString('token', authResponse.token);
    await prefs.setString('usuario', jsonEncode(authResponse.usuario.toJson()));
  }

  Future<void> logout() async {
    final prefs = await SharedPreferences.getInstance();
    await prefs.remove('token');
    await prefs.remove('usuario');
  }

  Future<String?> getToken() async {
    final prefs = await SharedPreferences.getInstance();
    return prefs.getString('token');
  }

  Future<Usuario?> getCurrentUser() async {
    final prefs = await SharedPreferences.getInstance();
    final usuarioStr = prefs.getString('usuario');
    if (usuarioStr != null) {
      return Usuario.fromJson(jsonDecode(usuarioStr));
    }
    return null;
  }
}
