package com.lowkeysdrops.web.service;

import com.lowkeysdrops.web.client.ApiClient;
import com.lowkeysdrops.web.dto.ProductoCreateRequest;
import com.lowkeysdrops.web.dto.ProductoResponse;
import com.lowkeysdrops.web.dto.ProductoUpdateRequest;
import org.springframework.stereotype.Service;

import java.util.Arrays;
import java.util.List;

@Service
public class ProductoService {
    
    private final ApiClient apiClient;

    public ProductoService(ApiClient apiClient) {
        this.apiClient = apiClient;
    }

    public List<ProductoResponse> getAll() {
        return Arrays.asList(apiClient.get("/api/Productos", ProductoResponse[].class));
    }

    public ProductoResponse getById(Integer id) {
        return apiClient.get("/api/Productos/" + id, ProductoResponse.class);
    }

    public void create(ProductoCreateRequest request) {
        apiClient.post("/api/Productos", request);
    }

    public void update(Integer id, ProductoUpdateRequest request) {
        apiClient.put("/api/Productos/" + id, request);
    }

    public void delete(Integer id) {
        apiClient.delete("/api/Productos/" + id);
    }
}
