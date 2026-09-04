class Usuario {
  final int idUsuario;
  final String nombre;
  final String correo;
  final String? telefono;
  final String? dui;
  final String rol;
  final bool estado;

  Usuario({
    required this.idUsuario,
    required this.nombre,
    required this.correo,
    this.telefono,
    this.dui,
    required this.rol,
    required this.estado,
  });

  factory Usuario.fromJson(Map<String, dynamic> json) {
    return Usuario(
      idUsuario: json['idUsuario'],
      nombre: json['nombre'],
      correo: json['correo'],
      telefono: json['telefono'],
      dui: json['dui'],
      rol: json['rol'],
      estado: json['estado'] ?? true,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'idUsuario': idUsuario,
      'nombre': nombre,
      'correo': correo,
      'telefono': telefono,
      'dui': dui,
      'rol': rol,
      'estado': estado,
    };
  }
}
