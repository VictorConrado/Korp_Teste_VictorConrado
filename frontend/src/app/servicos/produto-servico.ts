import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { Produto } from '../modelos/produto';

@Injectable({
  providedIn: 'root'
})
export class ProdutoServico {

  private readonly http = inject(HttpClient);

  private readonly url =
    'http://localhost:5174/api/produtos';

  listar(): Observable<Produto[]> {
    return this.http.get<Produto[]>(this.url);
  }
}