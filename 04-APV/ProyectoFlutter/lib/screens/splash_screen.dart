import 'package:flutter/material.dart';
import '../services/auth_service.dart';
import 'catalog_screen.dart';
import 'repartidor_dashboard_screen.dart';

class SplashScreen extends StatefulWidget {
  const SplashScreen({super.key});

  @override
  State<SplashScreen> createState() => _SplashScreenState();
}

class _SplashScreenState extends State<SplashScreen> {
  @override
  void initState() {
    super.initState();
    _checkAuth();
  }

  Future<void> _checkAuth() async {
    await Future.delayed(const Duration(seconds: 2));
    final authService = AuthService();
    final user = await authService.getCurrentUser();

    if (!mounted) return;

    if (user != null && user.rol.toUpperCase() == 'REPARTIDOR') {
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (_) => RepartidorDashboardScreen(usuario: user)),
      );
    } else {
      // Clientes o invitados (null) van al catálogo
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (_) => const CatalogScreen()),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            const Text(
              'LOWKEYS DROPS',
              style: TextStyle(
                fontSize: 32,
                fontWeight: FontWeight.bold,
                letterSpacing: 4.0,
              ),
            ),
            const SizedBox(height: 24),
            CircularProgressIndicator(color: Theme.of(context).colorScheme.primary),
          ],
        ),
      ),
    );
  }
}
