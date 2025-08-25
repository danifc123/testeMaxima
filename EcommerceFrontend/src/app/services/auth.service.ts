import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Usuario, UsuarioDto, LoginDto, AuthResponse } from '../models/usuario';
import { APP_CONSTANTS } from '../constants/app.constants';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = `${APP_CONSTANTS.API_BASE_URL}/auth`;

    constructor(private http: HttpClient) { }

    // Registra um novo usuário
    register(usuario: UsuarioDto): Observable<{ usuario: Usuario; message: string }> {
        return this.http.post<{ usuario: Usuario; message: string }>(`${this.apiUrl}/register`, usuario);
    }

    // Realiza login
    login(loginData: LoginDto): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${this.apiUrl}/login`, loginData);
    }

    // Busca usuário por ID
    getUsuario(id: string): Observable<Usuario> {
        return this.http.get<Usuario>(`${this.apiUrl}/usuario/${id}`);
    }

    // Salva token no localStorage
    saveToken(token: string): void {
        localStorage.setItem('auth_token', token);
    }

    // Obtém token do localStorage
    getToken(): string | null {
        return localStorage.getItem('auth_token');
    }

    // Remove token do localStorage
    removeToken(): void {
        localStorage.removeItem('auth_token');
    }

    // Verifica se o usuário está logado
    isLoggedIn(): boolean {
        return !!this.getToken();
    }

    // Salva dados do usuário no localStorage
    saveUser(usuario: Usuario): void {
        localStorage.setItem('user_data', JSON.stringify(usuario));
    }

    // Obtém dados do usuário do localStorage
    getUser(): Usuario | null {
        const userData = localStorage.getItem('user_data');
        return userData ? JSON.parse(userData) : null;
    }

    // Remove dados do usuário do localStorage
    removeUser(): void {
        localStorage.removeItem('user_data');
    }

    // Logout completo
    logout(): void {
        this.removeToken();
        this.removeUser();
    }
}
