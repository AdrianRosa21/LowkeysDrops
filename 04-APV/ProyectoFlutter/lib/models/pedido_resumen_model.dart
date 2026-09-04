class PedidoResumen {
  final int idPedido;
  final String fechaPedido;
  final String estadoPedido;
  final int idCliente;
  final String cliente;
  final String telefono;
  final String tipoDireccion;
  final String departamento;
  final String municipio;
  final String direccionTexto;
  final double subtotal;
  final double costoEnvio;
  final double total;
  final String? metodoPago;
  final String? estadoPago;
  final int? idEntrega;
  final int? idRepartidor;
  final String? repartidor;
  final String? estadoEntrega;
  final bool? confirmadoCliente;

  PedidoResumen({
    required this.idPedido,
    required this.fechaPedido,
    required this.estadoPedido,
    required this.idCliente,
    required this.cliente,
    required this.telefono,
    required this.tipoDireccion,
    required this.departamento,
    required this.municipio,
    required this.direccionTexto,
    required this.subtotal,
    required this.costoEnvio,
    required this.total,
    this.metodoPago,
    this.estadoPago,
    this.idEntrega,
    this.idRepartidor,
    this.repartidor,
    this.estadoEntrega,
    this.confirmadoCliente,
  });

  factory PedidoResumen.fromJson(Map<String, dynamic> json) {
    return PedidoResumen(
      idPedido: json['idPedido'] ?? 0,
      fechaPedido: json['fechaPedido'] ?? '',
      estadoPedido: json['estadoPedido'] ?? '',
      idCliente: json['idCliente'] ?? 0,
      cliente: json['cliente'] ?? '',
      telefono: json['telefono'] ?? '',
      tipoDireccion: json['tipoDireccion'] ?? '',
      departamento: json['departamento'] ?? '',
      municipio: json['municipio'] ?? '',
      direccionTexto: json['direccionTexto'] ?? '',
      subtotal: (json['subtotal'] as num?)?.toDouble() ?? 0.0,
      costoEnvio: (json['costoEnvio'] as num?)?.toDouble() ?? 0.0,
      total: (json['total'] as num?)?.toDouble() ?? 0.0,
      metodoPago: json['metodoPago'],
      estadoPago: json['estadoPago'],
      idEntrega: json['idEntrega'],
      idRepartidor: json['idRepartidor'],
      repartidor: json['repartidor'],
      estadoEntrega: json['estadoEntrega'],
      confirmadoCliente: json['confirmadoCliente'],
    );
  }
}
