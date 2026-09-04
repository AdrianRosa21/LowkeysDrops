package com.lowkeysdrops.web.service;

import com.lowkeysdrops.web.client.ApiClient;
import com.lowkeysdrops.web.dto.DireccionRequest;
import com.lowkeysdrops.web.dto.DireccionResponse;
import org.springframework.stereotype.Service;

import java.util.Arrays;
import java.util.List;
import java.util.stream.Collectors;

@Service
public class DireccionService {
    
    private final ApiClient apiClient;

    public DireccionService(ApiClient apiClient) {
        this.apiClient = apiClient;
    }

    public List<DireccionResponse> getByUsuarioId(Integer idUsuario) {
        DireccionResponse[] all = apiClient.get("/api/Direcciones", DireccionResponse[].class);
        return Arrays.stream(all)
                .filter(d -> idUsuario.equals(d.getIdUsuario()))
                .collect(Collectors.toList());
    }

    public DireccionResponse getById(Integer id) {
        return apiClient.get("/api/Direcciones/" + id, DireccionResponse.class);
    }

    public void create(DireccionRequest request) {
        apiClient.post("/api/Direcciones", request);
    }

    public void update(Integer id, DireccionRequest request) {
        apiClient.put("/api/Direcciones/" + id, request);
    }

    public void delete(Integer id) {
        apiClient.delete("/api/Direcciones/" + id);
    }
}
