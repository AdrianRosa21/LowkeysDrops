package com.lowkeysdrops.web.controller;

import com.lowkeysdrops.web.dto.DropRequest;
import com.lowkeysdrops.web.exception.ApiException;
import com.lowkeysdrops.web.service.DropService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

@Controller
@RequestMapping("/admin/drops")
public class DropController {

    private final DropService dropService;

    public DropController(DropService dropService) {
        this.dropService = dropService;
    }

    @GetMapping
    public String index(Model model) {
        try {
            model.addAttribute("drops", dropService.getAll());
        } catch (ApiException e) {
            model.addAttribute("error", "Error al cargar drops: " + e.getMessage());
        }
        return "drops/index";
    }

    @GetMapping("/nuevo")
    public String createForm(Model model) {
        model.addAttribute("drop", new DropRequest());
        return "drops/form";
    }

    @PostMapping("/nuevo")
    public String create(@ModelAttribute DropRequest request, RedirectAttributes redirectAttributes, Model model) {
        try {
            dropService.create(request);
            redirectAttributes.addFlashAttribute("success", "Drop creado correctamente.");
            return "redirect:/admin/drops";
        } catch (ApiException e) {
            model.addAttribute("error", "Error al crear: " + e.getMessage());
            model.addAttribute("drop", request);
            return "drops/form";
        }
    }

    @GetMapping("/{id}/editar")
    public String editForm(@PathVariable Integer id, Model model) {
        try {
            var d = dropService.getById(id);
            DropRequest req = new DropRequest();
            req.setNombre(d.getNombre());
            req.setDescripcion(d.getDescripcion());
            req.setFechaPublicacion(d.getFechaPublicacion() != null ? d.getFechaPublicacion().split("T")[0] : null);
            req.setEstado(d.getEstado());
            model.addAttribute("drop", req);
            model.addAttribute("idDrop", id);
            return "drops/form";
        } catch (ApiException e) {
            model.addAttribute("error", "Error al cargar drop: " + e.getMessage());
            return "drops/index";
        }
    }

    @PostMapping("/{id}/editar")
    public String edit(@PathVariable Integer id, @ModelAttribute DropRequest request, RedirectAttributes redirectAttributes, Model model) {
        try {
            dropService.update(id, request);
            redirectAttributes.addFlashAttribute("success", "Drop actualizado correctamente.");
            return "redirect:/admin/drops";
        } catch (ApiException e) {
            model.addAttribute("error", "Error al actualizar: " + e.getMessage());
            model.addAttribute("drop", request);
            model.addAttribute("idDrop", id);
            return "drops/form";
        }
    }

    @PostMapping("/{id}/eliminar")
    public String delete(@PathVariable Integer id, RedirectAttributes redirectAttributes) {
        try {
            dropService.delete(id);
            redirectAttributes.addFlashAttribute("success", "Drop eliminado correctamente.");
        } catch (ApiException e) {
            redirectAttributes.addFlashAttribute("error", "Error al eliminar: " + e.getMessage());
        }
        return "redirect:/admin/drops";
    }
}
