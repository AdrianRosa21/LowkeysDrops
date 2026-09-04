package com.lowkeysdrops.web.controller;

import com.lowkeysdrops.web.client.ApiClient;
import jakarta.servlet.http.HttpSession;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
@RequestMapping("/cliente")
public class ClienteController {
    
    private final ApiClient apiClient;

    public ClienteController(ApiClient apiClient) {
        this.apiClient = apiClient;
    }

    private Integer getUserId(HttpSession session) {
        return (Integer) session.getAttribute("USER_ID");
    }

    @GetMapping
    public String index() {
        return "cliente/index";
    }

    @GetMapping("/mis-pedidos")
    public String misPedidos(HttpSession session, Model model) {
        try {
            Integer userId = getUserId(session);
            var pedidos = apiClient.get("/api/Pedidos/cliente/" + userId, Object[].class);
            model.addAttribute("pedidos", pedidos);
            return "cliente/mis-pedidos";
        } catch (Exception e) {
            model.addAttribute("error", "Error al cargar pedidos: " + e.getMessage());
            return "cliente/index";
        }
    }

    @GetMapping("/perfil")
    public String perfilForm(HttpSession session, Model model) {
        try {
            var usuario = apiClient.get("/api/Auth/me", Object.class);
            model.addAttribute("usuario", usuario);
            return "cliente/perfil";
        } catch (Exception e) {
            model.addAttribute("error", "Error al cargar tu perfil: " + e.getMessage());
            return "cliente/index";
        }
    }

    @PostMapping("/perfil")
    public String updatePerfil(@org.springframework.web.bind.annotation.RequestParam String nombre,
                               @org.springframework.web.bind.annotation.RequestParam String correo,
                               @org.springframework.web.bind.annotation.RequestParam String telefono,
                               @org.springframework.web.bind.annotation.RequestParam String dui,
                               org.springframework.web.servlet.mvc.support.RedirectAttributes redirectAttributes) {
        try {
            var request = java.util.Map.of(
                "nombre", nombre,
                "correo", correo,
                "telefono", telefono,
                "dui", dui
            );
            apiClient.put("/api/Auth/me", request);
            redirectAttributes.addFlashAttribute("success", "Perfil actualizado correctamente.");
            return "redirect:/cliente";
        } catch (Exception e) {
            redirectAttributes.addFlashAttribute("error", "Error al actualizar perfil: " + e.getMessage());
            return "redirect:/cliente/perfil";
        }
    }
}
