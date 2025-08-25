export interface Usuario {
    id: string;
    nome: string;
    email: string;
    dataCriacao: Date;
}

export interface UsuarioDto {
    nome: string;
    email: string;
    senha: string;
}

export interface LoginDto {
    email: string;
    senha: string;
}

export interface LoginResponse {
    token: string;
    usuario: Usuario;
}

export interface AuthResponse {
    data: LoginResponse;
    message: string;
}
