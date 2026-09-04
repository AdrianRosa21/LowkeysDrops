class Direccion {
  final int idDireccion;
  final int idUsuario;
  final String tipo;
  final String departamento;
  final String municipio;
  final String direccionTexto;
  final String? referencia;

  Direccion({
    required this.idDireccion,
    required this.idUsuario,
    required this.tipo,
    required this.departamento,
    required this.municipio,
    required this.direccionTexto,
    this.referencia,
  });

  factory Direccion.fromJson(Map<String, dynamic> json) {
    return Direccion(
      idDireccion: json['idDireccion'],
      idUsuario: json['idUsuario'],
      tipo: json['tipo'],
      departamento: json['departamento'],
      municipio: json['municipio'],
      direccionTexto: json['direccionTexto'],
      referencia: json['referencia'],
    );
  }
}
