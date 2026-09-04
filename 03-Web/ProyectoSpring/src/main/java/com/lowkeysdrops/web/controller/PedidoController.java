package com.lowkeysdrops.web.controller;

import com.lowkeysdrops.web.dto.*;
import com.lowkeysdrops.web.exception.ApiException;
import com.lowkeysdrops.web.service.DireccionService;
import com.lowkeysdrops.web.service.PedidoService;
import com.lowkeysdrops.web.service.ProductoService;
import jakarta.servlet.http.HttpSession;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

@Controller
@RequestMapping("/cliente/pedidos")
public class PedidoController {

    private final PedidoService pedidoService;
    private final DireccionService direccionService;
    private final ProductoService productoService;

    public PedidoController(PedidoService pedidoService, DireccionService direccionService, ProductoService productoService) {
        this.pedidoService = pedidoService;
        this.direccionService = direccionService;
        this.productoService = productoService;
    }

    private Integer getUserId(HttpSession session) {
        return (Integer) session.getAttribute("USER_ID");
    }

    @GetMapping("/nuevo")
    public String createForm(@RequestParam Integer idProducto, HttpSession session, Model model, RedirectAttributes redirectAttributes) {
        try {
            var producto = productoService.getById(idProducto);
            if (!"DISPONIBLE".equals(producto.getEstado()) || producto.getStock() <= 0) {
                redirectAttributes.addFlashAttribute("error", "Este producto no está disponible.");
                return "redirect:/catalogo";
            }
            model.addAttribute("producto", producto);
            
            var direcciones = direccionService.getByUsuarioId(getUserId(session));
            if (direcciones.isEmpty()) {
                redirectAttributes.addFlashAttribute("error", "Debes registrar al menos una dirección antes de comprar.");
                return "redirect:/cliente/direcciones/nueva";
            }
            model.addAttribute("direcciones", direcciones);
            model.addAttribute("pedidoRequest", new PedidoCreateRequest());
            
            return "pedidos/form";
        } catch (ApiException e) {
            redirectAttributes.addFlashAttribute("error", "Error: " + e.getMessage());
            return "redirect:/catalogo";
        }
    }

    @PostMapping("/nuevo")
    public String create(@RequestParam Integer idProducto, @RequestParam Integer cantidad, @ModelAttribute PedidoCreateRequest request, HttpSession session, RedirectAttributes redirectAttributes, Model model) {
        try {
            request.setIdCliente(getUserId(session));
            
            // 1. Create order
            var response = pedidoService.crearPedido(request);
            Integer idPedido = response.getIdPedido();
            
            // 2. Add product
            AgregarProductoRequest agregarReq = new AgregarProductoRequest();
            agregarReq.setIdProducto(idProducto);
            agregarReq.setCantidad(cantidad);
            pedidoService.agregarProducto(idPedido, agregarReq);
            
            redirectAttributes.addFlashAttribute("success", "Pedido #" + idPedido + " creado correctamente.");
            return "redirect:/cliente/pedidos/" + idPedido;
        } catch (ApiException e) {
            redirectAttributes.addFlashAttribute("error", "No se pudo procesar tu pedido: " + e.getMessage());
            return "redirect:/cliente/pedidos/nuevo?idProducto=" + idProducto;
        }
    }

    @GetMapping("/{id}")
    public String verPedido(@PathVariable Integer id, HttpSession session, Model model) {
        try {
            var pedido = pedidoService.getById(id);
            if (!pedido.getIdCliente().equals(getUserId(session))) {
                model.addAttribute("error", "No tienes permiso para ver este pedido.");
                return "cliente/index";
            }
            model.addAttribute("pedido", pedido);
            return "pedidos/detalle";
        } catch (ApiException e) {
            model.addAttribute("error", "Error al cargar pedido: " + e.getMessage());
            return "cliente/index";
        }
    }

    @GetMapping("/rastrear")
    public String rastrearPedido(@RequestParam Integer id) {
        return "redirect:/cliente/pedidos/" + id;
    }

    @PostMapping("/{id}/confirmar-recepcion")
    public String confirmarRecepcion(@PathVariable Integer id, HttpSession session, RedirectAttributes redirectAttributes) {
        try {
            pedidoService.confirmarRecepcion(id, getUserId(session));
            redirectAttributes.addFlashAttribute("success", "Recepción del pedido confirmada.");
        } catch (ApiException e) {
            redirectAttributes.addFlashAttribute("error", "Error: " + e.getMessage());
        }
        return "redirect:/cliente/pedidos/" + id;
    }
}
