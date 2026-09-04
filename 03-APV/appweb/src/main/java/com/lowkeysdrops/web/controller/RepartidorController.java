package com.lowkeysdrops.web.controller;

import com.lowkeysdrops.web.client.ApiClient;
import com.lowkeysdrops.web.exception.ApiException;
import jakarta.servlet.http.HttpSession;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

import java.util.Map;

@Controller
@RequestMapping("/repartidor")
public class RepartidorController {

    private final ApiClient apiClient;

    public RepartidorController(ApiClient apiClient) {
        this.apiClient = apiClient;
    }

    private Integer getRepartidorId(HttpSession session) {
        return (Integer) session.getAttribute("USER_ID");
    }

    @GetMapping
    public String dashboard(HttpSession session, Model model) {
        model.addAttribute("ultimoPedido", session.getAttribute("ULTIMO_PEDIDO"));
        try {
            var historial = apiClient.get("/api/Repartidor/pedidos/" + getRepartidorId(session), Object[].class);
            model.addAttribute("historial", historial);
        } catch (ApiException e) {
            model.addAttribute("error", "No se pudo cargar el historial: " + e.getMessage());
        }
        return "repartidor/index";
    }

    @GetMapping("/pedidos")
    public String pedidosDisponibles(Model model) {
        try {
            var pedidos = apiClient.get("/api/Repartidor/pedidos-disponibles", Object[].class);
            model.addAttribute("pedidos", pedidos);
        } catch (ApiException e) {
            model.addAttribute("error", e.getMessage());
        }
        return "repartidor/pedidos";
    }

    @PostMapping("/pedidos/{id}/tomar")
    public String tomarPedido(@PathVariable Integer id, HttpSession session, RedirectAttributes redirectAttributes) {
        try {
            apiClient.post("/api/Pedidos/" + id + "/tomar", Map.of("idRepartidor", getRepartidorId(session)));
            session.setAttribute("ULTIMO_PEDIDO", id);
            redirectAttributes.addFlashAttribute("success", "Has tomado el pedido #" + id);
            return "redirect:/repartidor/pedidos/" + id + "/detalle";
        } catch (ApiException e) {
            redirectAttributes.addFlashAttribute("error", e.getMessage());
            return "redirect:/repartidor/pedidos";
        }
    }

    @GetMapping("/pedidos/{id}/detalle")
    public String detallePedido(@PathVariable Integer id, HttpSession session, Model model) {
        try {
            var pedido = apiClient.get("/api/Pedidos/" + id, Object.class);
            model.addAttribute("pedido", pedido);
            return "repartidor/detalle";
        } catch (ApiException e) {
            model.addAttribute("error", e.getMessage());
            return "repartidor/index";
        }
    }

    @PostMapping("/pedidos/{id}/en-camino")
    public String marcarEnCamino(@PathVariable Integer id, HttpSession session, RedirectAttributes redirectAttributes) {
        try {
            apiClient.put("/api/Pedidos/" + id + "/en-camino", Map.of("idRepartidor", getRepartidorId(session)));
            redirectAttributes.addFlashAttribute("success", "Pedido en camino.");
        } catch (ApiException e) {
            redirectAttributes.addFlashAttribute("error", e.getMessage());
        }
        return "redirect:/repartidor/pedidos/" + id + "/detalle";
    }

    @PostMapping("/pedidos/{id}/entrega")
    public String registrarEntrega(@PathVariable Integer id, @RequestParam String fotoEntregaUrl, @RequestParam(required = false) String observacion, HttpSession session, RedirectAttributes redirectAttributes) {
        try {
            apiClient.put("/api/Pedidos/" + id + "/entrega", Map.of("idRepartidor", getRepartidorId(session), "fotoEntregaUrl", fotoEntregaUrl, "observacion", observacion != null ? observacion : ""));
            session.removeAttribute("ULTIMO_PEDIDO");
            redirectAttributes.addFlashAttribute("success", "Entrega registrada correctamente.");
            return "redirect:/repartidor";
        } catch (ApiException e) {
            redirectAttributes.addFlashAttribute("error", e.getMessage());
            return "redirect:/repartidor/pedidos/" + id + "/detalle";
        }
    }

    @PostMapping("/pedidos/{id}/entrega-fallida")
    public String registrarEntregaFallida(@PathVariable Integer id, @RequestParam String observacion, HttpSession session, RedirectAttributes redirectAttributes) {
        try {
            apiClient.put("/api/Pedidos/" + id + "/entrega-fallida", Map.of("idRepartidor", getRepartidorId(session), "observacion", observacion));
            session.removeAttribute("ULTIMO_PEDIDO");
            redirectAttributes.addFlashAttribute("success", "Entrega fallida registrada.");
            return "redirect:/repartidor";
        } catch (ApiException e) {
            redirectAttributes.addFlashAttribute("error", e.getMessage());
            return "redirect:/repartidor/pedidos/" + id + "/detalle";
        }
    }
}
