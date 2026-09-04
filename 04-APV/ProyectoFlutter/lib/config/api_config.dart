class ApiConfig {
  // Use http://10.0.2.2:5069 for Android Emulator if the API is running locally
  // Use http://localhost:5069 for Windows/Web testing
  // Or replace with physical device's local IP address e.g. http://192.168.x.x:5069
  static const String baseUrl = 'http://10.0.2.2:5069/api';
  
  // URL of the Web App that serves the static images (Spring Boot usually runs on 8080)
  // IMPORTANTE: Si estás corriendo en Windows Desktop, cambia '10.0.2.2' por 'localhost'
  static const String webBaseUrl = 'http://10.0.2.2:8080';

  static String resolveImage(String? url) {
    if (url == null || url.isEmpty) return '';
    if (url.startsWith('http')) return url;
    if (!url.startsWith('/')) url = '/$url';
    return '$webBaseUrl$url';
  }
}
