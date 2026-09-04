class ProductoCatalogo {
  final int idProducto;
  final String nombre;
  final String? descripcion;
  final String? talla;
  final double precio;
  final String? imagenUrl;
  final bool esUnico;
  final int stock;
  final String estado;
  final String categoria;
  final String dropNombre;
  final int cantidadResenas;
  final double? promedioCalificacion;

  ProductoCatalogo({
    required this.idProducto,
    required this.nombre,
    this.descripcion,
    this.talla,
    required this.precio,
    this.imagenUrl,
    required this.esUnico,
    required this.stock,
    required this.estado,
    required this.categoria,
    required this.dropNombre,
    required this.cantidadResenas,
    this.promedioCalificacion,
  });

  factory ProductoCatalogo.fromJson(Map<String, dynamic> json) {
    return ProductoCatalogo(
      idProducto: json['idProducto'] ?? 0,
      nombre: json['nombre'] ?? '',
      descripcion: json['descripcion'],
      talla: json['talla'],
      precio: (json['precio'] as num?)?.toDouble() ?? 0.0,
      imagenUrl: json['imagenUrl'],
      esUnico: json['esUnico'] ?? false,
      stock: json['stock'] ?? 0,
      estado: json['estado'] ?? '',
      categoria: json['categoria'] ?? '',
      dropNombre: json['dropNombre'] ?? '',
      cantidadResenas: json['cantidadResenas'] ?? 0,
      promedioCalificacion: (json['promedioCalificacion'] as num?)?.toDouble(),
    );
  }
}
