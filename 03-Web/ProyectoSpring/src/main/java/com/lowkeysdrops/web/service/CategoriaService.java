package com.lowkeysdrops.web.service;

import com.lowkeysdrops.web.client.ApiClient;
import com.lowkeysdrops.web.dto.CategoriaRequest;
import com.lowkeysdrops.web.dto.CategoriaResponse;
import org.springframework.stereotype.Service;

import java.util.Arrays;
import java.util.List;

@Service
public class CategoriaService {
    
    private final ApiClient apiClient;

    public CategoriaService(ApiClient apiClient) {
        this.apiClient = apiClient;
    }

    public List<CategoriaResponse> getAll() {
        return Arrays.asList(apiClient.get("/api/Categorias", CategoriaResponse[].class));
    }

    public CategoriaResponse getById(Integer id) {
        return apiClient.get("/api/Categorias/" + id, CategoriaResponse.class);
    }

    public void create(CategoriaRequest request) {
        apiClient.post("/api/Categorias", request);
    }

    public void update(Integer id, CategoriaRequest request) {
        apiClient.put("/api/Categorias/" + id, request);
    }

    public void delete(Integer id) {
        apiClient.delete("/api/Categorias/" + id);
    }
}
