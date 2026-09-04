package com.lowkeysdrops.web.service;

import com.lowkeysdrops.web.client.ApiClient;
import com.lowkeysdrops.web.dto.CatalogoItemResponse;
import org.springframework.stereotype.Service;

import java.util.Arrays;
import java.util.List;

@Service
public class CatalogoService {
    
    private final ApiClient apiClient;

    public CatalogoService(ApiClient apiClient) {
        this.apiClient = apiClient;
    }

    public List<CatalogoItemResponse> getCatalogo() {
        CatalogoItemResponse[] items = apiClient.get("/api/Catalogo", CatalogoItemResponse[].class);
        return Arrays.asList(items);
    }
}
