package com.lowkeysdrops.web.dto;

public class DireccionRequest {
    private Integer idUsuario;
    private String tipo;
    private String departamento;
    private String municipio;
    private String direccionTexto;
    private String referencia;

    public Integer getIdUsuario() { return idUsuario; }
    public void setIdUsuario(Integer idUsuario) { this.idUsuario = idUsuario; }
    public String getTipo() { return tipo; }
    public void setTipo(String tipo) { this.tipo = tipo; }
    public String getDepartamento() { return departamento; }
    public void setDepartamento(String departamento) { this.departamento = departamento; }
    public String getMunicipio() { return municipio; }
    public void setMunicipio(String municipio) { this.municipio = municipio; }
    public String getDireccionTexto() { return direccionTexto; }
    public void setDireccionTexto(String direccionTexto) { this.direccionTexto = direccionTexto; }
    public String getReferencia() { return referencia; }
    public void setReferencia(String referencia) { this.referencia = referencia; }
}
