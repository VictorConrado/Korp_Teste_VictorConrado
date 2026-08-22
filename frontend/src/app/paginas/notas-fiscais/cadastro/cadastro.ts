import { CurrencyPipe } from '@angular/common';

import { Component, OnInit, inject } from '@angular/core';

import { FormsModule } from '@angular/forms';

import { Router, RouterLink } from '@angular/router';

import { Produto } from '../../../modelos/produto';

import {
  CriarNotaFiscalRequisicao,
  NotaFiscalServico
} from '../../../servicos/nota-fiscal-servico';

import { ProdutoServico } from '../../../servicos/produto-servico';

interface ItemNotaFiscalFormulario {
  produto: Produto;
  quantidade: number;
  valorUnitario: number;
  valorTotal: number;
}

@Component({
  selector: 'app-cadastro',
  imports: [
    CurrencyPipe,
    FormsModule,
    RouterLink
  ],
  templateUrl: './cadastro.html',
  styleUrl: './cadastro.css'
})
export class Cadastro implements OnInit {

  private readonly produtoServico = inject(ProdutoServico);

  private readonly notaFiscalServico = inject(NotaFiscalServico);

  private readonly roteador = inject(Router);

  produtos: Produto[] = [];

  itens: ItemNotaFiscalFormulario[] = [];

  produtoSelecionadoId: number | null = null;

  quantidade = 1;

  valorUnitario = 0;

  carregandoProdutos = true;

  salvando = false;

  mensagemErro = '';

  estoqueIndisponivel = false;

  ngOnInit(): void {
    this.carregarProdutos();
  }

  carregarProdutos(): void {

    this.carregandoProdutos = true;

    this.mensagemErro = '';

    this.estoqueIndisponivel = false;

    this.produtoServico.listar().subscribe({

      next: produtos => {

        this.produtos = produtos;

        this.carregandoProdutos = false;

      },

      error: erro => {

        console.error(
          'Erro ao carregar produtos:',
          erro
        );

        this.carregandoProdutos = false;

        if (erro?.status === 503) {

          this.estoqueIndisponivel = true;

          this.mensagemErro =
            'O serviço de estoque está temporariamente indisponível.';

        } else {

          this.mensagemErro =
            'Não foi possível carregar os produtos.';

        }

      }

    });
  }

  produtoSelecionado(): Produto | undefined {

    return this.produtos.find(
      produto => produto.id === this.produtoSelecionadoId
    );
  }

  adicionarItem(): void {

    this.mensagemErro = '';

    const produto = this.produtoSelecionado();

    if (!produto) {

      this.mensagemErro =
        'Selecione um produto.';

      return;
    }

    if (this.quantidade <= 0) {

      this.mensagemErro =
        'A quantidade deve ser maior que zero.';

      return;
    }

    if (this.valorUnitario < 0) {

      this.mensagemErro =
        'O valor unitário não pode ser negativo.';

      return;
    }

    const itemExistente = this.itens.find(
      item => item.produto.id === produto.id
    );

    if (itemExistente) {

      itemExistente.quantidade += this.quantidade;

      itemExistente.valorTotal =
        itemExistente.quantidade *
        itemExistente.valorUnitario;

    } else {

      this.itens.push({
        produto,
        quantidade: this.quantidade,
        valorUnitario: this.valorUnitario,
        valorTotal:
          this.quantidade *
          this.valorUnitario
      });

    }

    this.limparItem();
  }

  removerItem(indice: number): void {

    this.itens.splice(indice, 1);
  }

  limparItem(): void {

    this.produtoSelecionadoId = null;

    this.quantidade = 1;

    this.valorUnitario = 0;
  }

  obterTotal(): number {

    return this.itens.reduce(
      (total, item) => total + item.valorTotal,
      0
    );
  }

  criarNota(): void {

    this.mensagemErro = '';

    if (this.itens.length === 0) {

      this.mensagemErro =
        'Adicione pelo menos um item à nota fiscal.';

      return;
    }

    const requisicao: CriarNotaFiscalRequisicao = {

      itens: this.itens.map(item => ({

        produtoId: item.produto.id,

        quantidade: item.quantidade,

        valorUnitario: item.valorUnitario

      }))

    };

    this.salvando = true;

    this.notaFiscalServico.criar(requisicao).subscribe({

      next: nota => {

        this.salvando = false;

        this.roteador.navigate([
          '/notas-fiscais'
        ]);

      },

      error: erro => {

        console.error(
          'Erro ao criar nota fiscal:',
          erro
        );

        this.mensagemErro =
          erro?.error?.mensagem ??
          'Não foi possível criar a nota fiscal.';

        this.salvando = false;

      }

    });
  }
}