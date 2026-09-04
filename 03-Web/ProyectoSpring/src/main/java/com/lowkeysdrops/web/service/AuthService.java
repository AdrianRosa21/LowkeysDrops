package com.lowkeysdrops.web.service;

import com.lowkeysdrops.web.client.ApiClient;
import com.lowkeysdrops.web.dto.AuthResponse;
import com.lowkeysdrops.web.dto.LoginRequest;
import com.lowkeysdrops.web.dto.RegistroRequest;
import org.springframework.stereotype.Service;

@Service
public class AuthService {
    private final ApiClient apiClient;

    public AuthService(ApiClient apiClient) {
        this.apiClient = apiClient;
    }

    public AuthResponse login(LoginRequest request) {
        return apiClient.post("/api/Auth/login", request, AuthResponse.class);
    }

    public void registrar(RegistroRequest request) {
        apiClient.post("/api/Auth/registro", request);
    }
}
