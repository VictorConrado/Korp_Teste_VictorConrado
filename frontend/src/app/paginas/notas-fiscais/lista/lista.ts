import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { NotaFiscalServico } from '../../../servicos/nota-fiscal-servico';
import { NotaFiscal } from '../../../modelos/nota-fiscal';

@Component({
  selector: 'app-lista',
  imports: [CommonModule, RouterLink],
  templateUrl: './lista.html',
  styleUrl: './lista.css'
})
export class Lista implements OnInit {

  private readonly notaFiscalServico = inject(NotaFiscalServico);

  notasFiscais: NotaFiscal[] = [];

  carregando = true;

  mensagemErro = '';

  mensagemSucesso = '';

  imprimindoId: number | null = null;

  ngOnInit(): void {
    this.carregarNotas();
  }

  carregarNotas(): void {
    this.carregando = true;
    this.mensagemErro = '';

    this.notaFiscalServico.listar().subscribe({
      next: notas => {
        this.notasFiscais = notas;
        this.carregando = false;
      },

      error: erro => {
        console.error(
          'Erro ao carregar notas fiscais:',
          erro
        );

        this.mensagemErro =
          'Não foi possível carregar as notas fiscais.';

        this.carregando = false;
      }
    });
  }

  imprimir(nota: NotaFiscal): void {

    if (
      nota.status === 3 ||
      this.imprimindoId !== null
    ) {
      return;
    }

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    this.imprimindoId = nota.id;

    this.notaFiscalServico.imprimir(nota.id).subscribe({

      next: notaAtualizada => {

        const indice =
          this.notasFiscais.findIndex(
            item => item.id === nota.id
          );

        if (indice !== -1) {
          this.notasFiscais[indice] = notaAtualizada;
        }

        this.imprimindoId = null;

        this.mensagemSucesso =
          `Nota fiscal ${notaAtualizada.numero} fechada com sucesso.`;
      },

      error: erro => {

        console.error(
          'Erro ao imprimir nota fiscal:',
          erro
        );

        this.imprimindoId = null;

        this.mensagemErro =
          erro?.error?.mensagem ??
          'Não foi possível imprimir a nota fiscal.';
      }
    });
  }

  obterNomeStatus(status: number): string {

    switch (status) {

      case 1:
        return 'Aberta';

      case 3:
        return 'Fechada';

      default:
        return 'Desconhecido';
    }
  }
}