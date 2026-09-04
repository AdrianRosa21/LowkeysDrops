import 'package:flutter/material.dart';
import '../models/pedido_resumen_model.dart';
import '../models/usuario_model.dart';
import '../services/repartidor_service.dart';
import '../services/auth_service.dart';
import 'login_screen.dart';
import 'repartidor_detail_screen.dart';

class RepartidorDashboardScreen extends StatefulWidget {
  final Usuario usuario;
  const RepartidorDashboardScreen({super.key, required this.usuario});

  @override
  State<RepartidorDashboardScreen> createState() => _RepartidorDashboardScreenState();
}

class _RepartidorDashboardScreenState extends State<RepartidorDashboardScreen> with SingleTickerProviderStateMixin {
  late TabController _tabController;
  final _repartidorService = RepartidorService();
  final _authService = AuthService();

  List<PedidoResumen> _disponibles = [];
  List<PedidoResumen> _historial = [];
  bool _isLoading = true;

  @override
  void initState() {
    super.initState();
    _tabController = TabController(length: 2, vsync: this);
    _loadData();
  }

  Future<void> _loadData() async {
    setState(() => _isLoading = true);
    try {
      final disponibles = await _repartidorService.getPedidosDisponibles();
      final historial = await _repartidorService.getHistorialPedidos(widget.usuario.idUsuario);
      setState(() {
        _disponibles = disponibles;
        _historial = historial.reversed.toList();
      });
    } catch (e) {
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(const SnackBar(content: Text('Error al cargar datos')));
      }
    } finally {
      if (mounted) setState(() => _isLoading = false);
    }
  }

  Future<void> _logout() async {
    await _authService.logout();
    if (!mounted) return;
    Navigator.pushReplacement(context, MaterialPageRoute(builder: (_) => const LoginScreen()));
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('PANEL REPARTIDOR'),
        actions: [
          IconButton(icon: const Icon(Icons.refresh), onPressed: _loadData),
          IconButton(icon: const Icon(Icons.logout), onPressed: _logout),
        ],
        bottom: TabBar(
          controller: _tabController,
          tabs: const [
            Tab(text: 'DISPONIBLES'),
            Tab(text: 'MI HISTORIAL'),
          ],
        ),
      ),
      body: _isLoading
          ? const Center(child: CircularProgressIndicator())
          : TabBarView(
              controller: _tabController,
              children: [
                _buildList(_disponibles, isDisponible: true),
                _buildList(_historial, isDisponible: false),
              ],
            ),
    );
  }

  Widget _buildList(List<PedidoResumen> pedidos, {required bool isDisponible}) {
    if (pedidos.isEmpty) {
      return Center(child: Text(isDisponible ? 'No hay pedidos disponibles.' : 'Tu historial está vacío.'));
    }
    return RefreshIndicator(
      onRefresh: _loadData,
      child: ListView.builder(
        padding: const EdgeInsets.all(16),
        itemCount: pedidos.length,
        itemBuilder: (context, index) {
          final pedido = pedidos[index];
          return Card(
            margin: const EdgeInsets.only(bottom: 12),
            child: ListTile(
              title: Text('Pedido #${pedido.idPedido} - ${pedido.cliente}'),
              subtitle: Text('${pedido.municipio}\nTotal: \$${pedido.total.toStringAsFixed(2)}'),
              trailing: Text(
                pedido.estadoPedido,
                style: const TextStyle(fontWeight: FontWeight.bold, color: Colors.blueAccent),
              ),
              isThreeLine: true,
              onTap: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(
                    builder: (_) => RepartidorDetailScreen(pedido: pedido, repartidorId: widget.usuario.idUsuario),
                  ),
                ).then((_) => _loadData());
              },
            ),
          );
        },
      ),
    );
  }
}
