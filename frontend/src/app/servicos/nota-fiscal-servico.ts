import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { NotaFiscal } from '../modelos/nota-fiscal';

export interface CriarNotaFiscalRequisicao {
  itens: {
    produtoId: number;
    quantidade: number;
    valorUnitario: number;
  }[];
}

@Injectable({
  providedIn: 'root'
})
export class NotaFiscalServico {

  private readonly http = inject(HttpClient);

  private readonly url =
    'http://localhost:5189/api/notas-fiscais';

  criar(
    requisicao: CriarNotaFiscalRequisicao
  ): Observable<NotaFiscal> {
    return this.http.post<NotaFiscal>(
      this.url,
      requisicao
    );
  }

  buscarPorId(id: number): Observable<NotaFiscal> {
    return this.http.get<NotaFiscal>(
      `${this.url}/${id}`
    );
  }

  imprimir(id: number): Observable<NotaFiscal> {
    return this.http.post<NotaFiscal>(
      `${this.url}/${id}/imprimir`,
      {}
    );
  }
}