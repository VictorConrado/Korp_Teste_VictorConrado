import { Routes } from '@angular/router';

import { Inicio } from './paginas/inicio/inicio';
import { Produtos } from './paginas/produtos/produtos';
import { Lista } from './paginas/notas-fiscais/lista/lista';
import { Cadastro } from './paginas/notas-fiscais/cadastro/cadastro';

export const routes: Routes = [
  {
    path: '',
    component: Inicio
  },
  {
    path: 'produtos',
    component: Produtos
  },
  {
    path: 'notas-fiscais',
    component: Lista
  },
  {
    path: 'notas-fiscais/nova',
    component: Cadastro
  }
];