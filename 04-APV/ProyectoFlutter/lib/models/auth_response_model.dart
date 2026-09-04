import 'usuario_model.dart';

class AuthResponse {
  final String token;
  final Usuario usuario;

  AuthResponse({required this.token, required this.usuario});

  factory AuthResponse.fromJson(Map<String, dynamic> json) {
    return AuthResponse(
      token: json['token'],
      usuario: Usuario.fromJson(json['usuario']),
    );
  }
}
