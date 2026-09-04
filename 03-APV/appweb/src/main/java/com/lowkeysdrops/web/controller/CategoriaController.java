package com.lowkeysdrops.web.controller;

import com.lowkeysdrops.web.dto.CategoriaRequest;
import com.lowkeysdrops.web.exception.ApiException;
import com.lowkeysdrops.web.service.CategoriaService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.mvc.support.RedirectAttributes;

@Controller
@RequestMapping("/admin/categorias")
public class CategoriaController {

    private final CategoriaService categoriaService;

    public CategoriaController(CategoriaService categoriaService) {
        this.categoriaService = categoriaService;
    }

    @GetMapping
    public String index(Model model) {
        try {
            model.addAttribute("categorias", categoriaService.getAll());
        } catch (ApiException e) {
            model.addAttribute("error", "Error al cargar categorías: " + e.getMessage());
        }
        return "categorias/index";
    }

    @GetMapping("/nueva")
    public String createForm(Model model) {
        model.addAttribute("categoria", new CategoriaRequest());
        return "categorias/form";
    }

    @PostMapping("/nueva")
    public String create(@ModelAttribute CategoriaRequest request, RedirectAttributes redirectAttributes, Model model) {
        try {
            categoriaService.create(request);
            redirectAttributes.addFlashAttribute("success", "Categoría creada correctamente.");
            return "redirect:/admin/categorias";
        } catch (ApiException e) {
            model.addAttribute("error", "Error al crear: " + e.getMessage());
            model.addAttribute("categoria", request);
            return "categorias/form";
        }
    }

    @GetMapping("/{id}/editar")
    public String editForm(@PathVariable Integer id, Model model) {
        try {
            var c = categoriaService.getById(id);
            CategoriaRequest req = new CategoriaRequest();
            req.setNombre(c.getNombre());
            req.setDescripcion(c.getDescripcion());
            req.setEstado(c.getEstado());
            model.addAttribute("categoria", req);
            model.addAttribute("idCategoria", id);
            return "categorias/form";
        } catch (ApiException e) {
            model.addAttribute("error", "Error al cargar categoría: " + e.getMessage());
            return "categorias/index";
        }
    }

    @PostMapping("/{id}/editar")
    public String edit(@PathVariable Integer id, @ModelAttribute CategoriaRequest request, RedirectAttributes redirectAttributes, Model model) {
        try {
            categoriaService.update(id, request);
            redirectAttributes.addFlashAttribute("success", "Categoría actualizada correctamente.");
            return "redirect:/admin/categorias";
        } catch (ApiException e) {
            model.addAttribute("error", "Error al actualizar: " + e.getMessage());
            model.addAttribute("categoria", request);
            model.addAttribute("idCategoria", id);
            return "categorias/form";
        }
    }

    @PostMapping("/{id}/eliminar")
    public String delete(@PathVariable Integer id, RedirectAttributes redirectAttributes) {
        try {
            categoriaService.delete(id);
            redirectAttributes.addFlashAttribute("success", "Categoría eliminada correctamente.");
        } catch (ApiException e) {
            redirectAttributes.addFlashAttribute("error", "Error al eliminar: " + e.getMessage());
        }
        return "redirect:/admin/categorias";
    }
}
