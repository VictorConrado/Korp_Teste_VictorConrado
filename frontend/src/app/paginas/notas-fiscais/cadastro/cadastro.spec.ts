import { TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';

import { Cadastro } from './cadastro';
import { ProdutoServico } from '../../../servicos/produto-servico';
import { NotaFiscalServico } from '../../../servicos/nota-fiscal-servico';

describe('Cadastro', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Cadastro],
      providers: [
        provideRouter([]),
        {
          provide: ProdutoServico,
          useValue: {
            listar: () => of([])
          }
        },
        {
          provide: NotaFiscalServico,
          useValue: {
            criar: () => of({})
          }
        }
      ]
    }).compileComponents();
  });

  it('deve criar o componente', () => {
    const fixture = TestBed.createComponent(Cadastro);
    const componente = fixture.componentInstance;

    expect(componente).toBeTruthy();
  });
});