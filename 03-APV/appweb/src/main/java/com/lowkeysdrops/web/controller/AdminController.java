package com.lowkeysdrops.web.controller;

import com.lowkeysdrops.web.client.ApiClient;
import com.lowkeysdrops.web.dto.RegistroRequest;
import com.lowkeysdrops.web.exception.ApiException;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

import java.util.Map;

@Controller
@RequestMapping("/admin")
public class AdminController {

    private final ApiClient apiClient;

    public AdminController(ApiClient apiClient) {
        this.apiClient = apiClient;
    }

    @GetMapping
    public String dashboard() {
        return "admin/index";
    }

    @GetMapping("/usuarios")
    public String usuarios(Model model) {
        try {
            var usuarios = apiClient.get("/api/Admin/usuarios", Object[].class);
            model.addAttribute("usuarios", usuarios);
        } catch (ApiException e) {
            model.addAttribute("error", e.getMessage());
        }
        return "admin/usuarios";
    }

    @PostMapping("/usuarios/{id}/estado")
    public String toggleEstadoUsuario(@PathVariable Integer id, @RequestParam Boolean activo, RedirectAttributes redirectAttributes) {
        try {
            apiClient.put("/api/Admin/usuarios/" + id + "/estado", Map.of("activo", activo));
            redirectAttributes.addFlashAttribute("success", "Estado actualizado correctamente.");
        } catch (ApiException e) {
            redirectAttributes.addFlashAttribute("error", e.getMessage());
        }
        return "redirect:/admin/usuarios";
    }

    @GetMapping("/repartidores/nuevo")
    public String crearRepartidorForm(Model model) {
        model.addAttribute("registroRequest", new RegistroRequest());
        return "admin/repartidor_form";
    }

    @PostMapping("/repartidores/nuevo")
    public String crearRepartidor(@ModelAttribute RegistroRequest request, RedirectAttributes redirectAttributes, Model model) {
        try {
            apiClient.post("/api/Admin/repartidores", request);
            redirectAttributes.addFlashAttribute("success", "Repartidor creado correctamente.");
            return "redirect:/admin/usuarios";
        } catch (ApiException e) {
            model.addAttribute("error", e.getMessage());
            model.addAttribute("registroRequest", request);
            return "admin/repartidor_form";
        }
    }

    @GetMapping("/pedidos")
    public String pedidos(Model model) {
        try {
            var pedidos = apiClient.get("/api/Admin/pedidos", Object[].class);
            model.addAttribute("pedidos", pedidos);
        } catch (ApiException e) {
            model.addAttribute("error", e.getMessage());
        }
        return "admin/pedidos";
    }

    @GetMapping("/ventas")
    public String ventas(Model model) {
        try {
            var ventas = apiClient.get("/api/Admin/ventas", Object[].class);
            model.addAttribute("ventas", ventas);
        } catch (ApiException e) {
            model.addAttribute("error", e.getMessage());
        }
        return "admin/ventas";
    }

    @GetMapping("/auditoria")
    public String auditoria(Model model) {
        try {
            var auditoria = apiClient.get("/api/Admin/auditoria", Object[].class);
            model.addAttribute("auditoria", auditoria);
        } catch (ApiException e) {
            model.addAttribute("error", e.getMessage());
        }
        return "admin/auditoria";
    }

    @GetMapping("/pedidos/{id}")
    public String verPedido(@PathVariable Integer id, Model model) {
        try {
            var pedido = apiClient.get("/api/Pedidos/" + id, Object.class);
            model.addAttribute("pedido", pedido);
            return "pedidos/detalle";
        } catch (ApiException e) {
            model.addAttribute("error", "Error al cargar pedido: " + e.getMessage());
            return "admin/index";
        }
    }

    @PostMapping("/pedidos/{id}/verificar-pago")
    public String verificarPago(@PathVariable Integer id, @RequestParam String referencia, RedirectAttributes redirectAttributes) {
        try {
            apiClient.put("/api/Pedidos/" + id + "/pago/verificar", Map.of("referencia", referencia));
            redirectAttributes.addFlashAttribute("success", "Pago verificado correctamente.");
        } catch (ApiException e) {
            redirectAttributes.addFlashAttribute("error", "Error: " + e.getMessage());
        }
        return "redirect:/admin/pedidos/" + id;
    }
}
