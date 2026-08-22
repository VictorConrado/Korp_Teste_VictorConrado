import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  Produto,
  CriarProdutoRequisicao,
  AtualizarProdutoRequisicao
} from '../modelos/produto';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProdutoServico {

  private http = inject(HttpClient);

  private readonly endereco = 'http://localhost:5174/api/produtos';

  listar(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.endereco);
  }

  buscarPorId(id: number): Observable<Produto> {
    return this.http.get<Produto>(`${this.endereco}/${id}`);
  }

  criar(requisicao: CriarProdutoRequisicao): Observable<Produto> {
    return this.http.post<Produto>(
      this.endereco,
      requisicao
    );
  }

  atualizar(
    id: number,
    requisicao: AtualizarProdutoRequisicao
  ): Observable<Produto> {
    return this.http.put<Produto>(
      `${this.endereco}/${id}`,
      requisicao
    );
  }

  excluir(id: number): Observable<void> {
    return this.http.delete<void>(
      `${this.endereco}/${id}`
    );
  }
}