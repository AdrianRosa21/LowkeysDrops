package com.lowkeysdrops.web.controller;

import com.lowkeysdrops.web.service.CatalogoService;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

@Controller
public class HomeController {

    private final CatalogoService catalogoService;

    public HomeController(CatalogoService catalogoService) {
        this.catalogoService = catalogoService;
    }

    @GetMapping("/")
    public String home(Model model) {
        try {
            var items = catalogoService.getCatalogo();
            // Show only a few highlighted items for the home page (e.g. up to 4)
            if (items.size() > 4) {
                model.addAttribute("destacados", items.subList(0, 4));
            } else {
                model.addAttribute("destacados", items);
            }
        } catch (Exception e) {
            model.addAttribute("error", "No se pudo cargar el catálogo. " + e.getMessage());
        }
        return "index";
    }

    @GetMapping("/catalogo")
    public String catalogo(Model model) {
        try {
            model.addAttribute("productos", catalogoService.getCatalogo());
        } catch (Exception e) {
            model.addAttribute("error", "No se pudo cargar el catálogo. " + e.getMessage());
        }
        return "catalogo/index";
    }
}
