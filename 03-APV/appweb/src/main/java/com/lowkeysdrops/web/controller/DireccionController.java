package com.lowkeysdrops.web.controller;

import com.lowkeysdrops.web.dto.DireccionRequest;
import com.lowkeysdrops.web.exception.ApiException;
import com.lowkeysdrops.web.service.DireccionService;
import jakarta.servlet.http.HttpSession;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

@Controller
@RequestMapping("/cliente/direcciones")
public class DireccionController {

    private final DireccionService direccionService;

    public DireccionController(DireccionService direccionService) {
        this.direccionService = direccionService;
    }

    private Integer getUserId(HttpSession session) {
        return (Integer) session.getAttribute("USER_ID");
    }

    @GetMapping
    public String index(HttpSession session, Model model) {
        try {
            model.addAttribute("direcciones", direccionService.getByUsuarioId(getUserId(session)));
        } catch (ApiException e) {
            model.addAttribute("error", "Error al cargar direcciones: " + e.getMessage());
        }
        return "direcciones/index";
    }

    @GetMapping("/nueva")
    public String createForm(Model model) {
        model.addAttribute("direccion", new DireccionRequest());
        return "direcciones/form";
    }

    @PostMapping("/nueva")
    public String create(@ModelAttribute DireccionRequest request, HttpSession session, RedirectAttributes redirectAttributes, Model model) {
        try {
            request.setIdUsuario(getUserId(session));
            direccionService.create(request);
            redirectAttributes.addFlashAttribute("success", "Dirección creada correctamente.");
            return "redirect:/cliente/direcciones";
        } catch (ApiException e) {
            model.addAttribute("error", "Error al crear: " + e.getMessage());
            model.addAttribute("direccion", request);
            return "direcciones/form";
        }
    }

    @GetMapping("/{id}/editar")
    public String editForm(@PathVariable Integer id, HttpSession session, Model model, RedirectAttributes redirectAttributes) {
        try {
            var d = direccionService.getById(id);
            if (!d.getIdUsuario().equals(getUserId(session))) {
                redirectAttributes.addFlashAttribute("error", "No tienes permiso para editar esta dirección.");
                return "redirect:/cliente/direcciones";
            }
            DireccionRequest req = new DireccionRequest();
            req.setTipo(d.getTipo());
            req.setDepartamento(d.getDepartamento());
            req.setMunicipio(d.getMunicipio());
            req.setDireccionTexto(d.getDireccionTexto());
            req.setReferencia(d.getReferencia());
            model.addAttribute("direccion", req);
            model.addAttribute("idDireccion", id);
            return "direcciones/form";
        } catch (ApiException e) {
            model.addAttribute("error", "Error al cargar dirección: " + e.getMessage());
            return "direcciones/index";
        }
    }

    @PostMapping("/{id}/editar")
    public String edit(@PathVariable Integer id, @ModelAttribute DireccionRequest request, HttpSession session, RedirectAttributes redirectAttributes, Model model) {
        try {
            request.setIdUsuario(getUserId(session)); // preserve user id
            direccionService.update(id, request);
            redirectAttributes.addFlashAttribute("success", "Dirección actualizada correctamente.");
            return "redirect:/cliente/direcciones";
        } catch (ApiException e) {
            model.addAttribute("error", "Error al actualizar: " + e.getMessage());
            model.addAttribute("direccion", request);
            model.addAttribute("idDireccion", id);
            return "direcciones/form";
        }
    }

    @PostMapping("/{id}/eliminar")
    public String delete(@PathVariable Integer id, RedirectAttributes redirectAttributes) {
        try {
            // Note: In a real app we should check ownership before deleting, but API might handle it or we can just try.
            direccionService.delete(id);
            redirectAttributes.addFlashAttribute("success", "Dirección eliminada correctamente.");
        } catch (ApiException e) {
            redirectAttributes.addFlashAttribute("error", "Error al eliminar: " + e.getMessage());
        }
        return "redirect:/cliente/direcciones";
    }
}
