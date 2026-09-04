package com.lowkeysdrops.web.interceptor;

import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;
import org.springframework.stereotype.Component;
import org.springframework.web.servlet.HandlerInterceptor;

@Component
public class AuthInterceptor implements HandlerInterceptor {

    @Override
    public boolean preHandle(HttpServletRequest request, HttpServletResponse response, Object handler) throws Exception {
        String uri = request.getRequestURI();
        
        // Allow public access
        if (uri.equals("/") || uri.startsWith("/catalogo") || uri.startsWith("/login") 
                || uri.startsWith("/registro") || uri.startsWith("/css") || uri.startsWith("/js") 
                || uri.startsWith("/img") || uri.startsWith("/error")) {
            return true;
        }

        HttpSession session = request.getSession(false);
        if (session == null || session.getAttribute("USER_ROLE") == null) {
            response.sendRedirect("/login");
            return false;
        }

        String role = (String) session.getAttribute("USER_ROLE");

        // Admin paths
        if (uri.startsWith("/admin") || uri.startsWith("/productos") || uri.startsWith("/categorias") || uri.startsWith("/drops")) {
            if (!"ADMIN".equals(role)) {
                response.sendRedirect("/error/403");
                return false;
            }
        }
        // Client paths
        else if (uri.startsWith("/cliente") || uri.startsWith("/direcciones") || uri.startsWith("/pedidos")) {
            if (!"CLIENTE".equals(role)) {
                response.sendRedirect("/error/403");
                return false;
            }
        }
        // Repartidor paths
        else if (uri.startsWith("/repartidor")) {
            if (!"REPARTIDOR".equals(role)) {
                response.sendRedirect("/error/403");
                return false;
            }
        }

        return true;
    }
}
