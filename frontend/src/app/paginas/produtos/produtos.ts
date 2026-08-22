import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  Produto,
  CriarProdutoRequisicao
} from '../../modelos/produto';

import { ProdutoServico } from '../../servicos/produto-servico';

@Component({
  selector: 'app-produtos',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule
  ],
  templateUrl: './produtos.html',
  styleUrl: './produtos.css'
})
export class Produtos implements OnInit {

  private produtoServico = inject(ProdutoServico);

  produtos: Produto[] = [];

  produtoPesquisa: Produto | null = null;

  idPesquisa = '';

  carregando = false;
  salvando = false;

  mensagemErro = '';
  mensagemSucesso = '';

  formularioAberto = false;
  modoEdicao = false;

  produtoIdEdicao: number | null = null;

  produtoFormulario: CriarProdutoRequisicao = {
    codigo: '',
    descricao: '',
    saldo: 0
  };

  ngOnInit(): void {
    this.carregarProdutos();
  }

  carregarProdutos(): void {
    this.carregando = true;
    this.mensagemErro = '';

    this.produtoServico.listar().subscribe({
      next: produtos => {
        this.produtos = produtos;
        this.carregando = false;
      },
      error: erro => {
        this.carregando = false;
        this.mensagemErro =
          this.obterMensagemErro(
            erro,
            'Não foi possível carregar os produtos.'
          );
      }
    });
  }

  pesquisar(): void {
    this.mensagemErro = '';
    this.mensagemSucesso = '';
    this.produtoPesquisa = null;

    const id = Number(this.idPesquisa);

    if (!id || id <= 0) {
      this.carregarProdutos();
      return;
    }

    this.carregando = true;

    this.produtoServico.buscarPorId(id).subscribe({
      next: produto => {
        this.produtoPesquisa = produto;
        this.produtos = [produto];
        this.carregando = false;
      },
      error: erro => {
        this.carregando = false;

        this.mensagemErro =
          this.obterMensagemErro(
            erro,
            `Produto com ID ${id} não foi encontrado.`
          );
      }
    });
  }

  limparPesquisa(): void {
    this.idPesquisa = '';
    this.produtoPesquisa = null;
    this.mensagemErro = '';
    this.carregarProdutos();
  }

  abrirCadastro(): void {
    this.modoEdicao = false;
    this.produtoIdEdicao = null;

    this.produtoFormulario = {
      codigo: '',
      descricao: '',
      saldo: 0
    };

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    this.formularioAberto = true;
  }

  abrirEdicao(produto: Produto): void {
    this.modoEdicao = true;
    this.produtoIdEdicao = produto.id;

    this.produtoFormulario = {
      codigo: produto.codigo,
      descricao: produto.descricao,
      saldo: produto.saldo
    };

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    this.formularioAberto = true;
  }

  fecharFormulario(): void {
    if (this.salvando) {
      return;
    }

    this.formularioAberto = false;
  }

  salvar(): void {
    this.mensagemErro = '';
    this.mensagemSucesso = '';

    if (
      !this.produtoFormulario.codigo.trim() ||
      !this.produtoFormulario.descricao.trim()
    ) {
      this.mensagemErro =
        'Código e descrição são obrigatórios.';
      return;
    }

    if (this.produtoFormulario.saldo < 0) {
      this.mensagemErro =
        'O saldo não pode ser negativo.';
      return;
    }

    this.salvando = true;

    if (this.modoEdicao && this.produtoIdEdicao !== null) {

      this.produtoServico
        .atualizar(
          this.produtoIdEdicao,
          this.produtoFormulario
        )
        .subscribe({
          next: () => {
            this.salvando = false;
            this.formularioAberto = false;

            this.mensagemSucesso =
              'Produto atualizado com sucesso.';

            this.carregarProdutos();
          },
          error: erro => {
            this.salvando = false;

            this.mensagemErro =
              this.obterMensagemErro(
                erro,
                'Não foi possível atualizar o produto.'
              );
          }
        });

      return;
    }

    this.produtoServico
      .criar(this.produtoFormulario)
      .subscribe({
        next: () => {
          this.salvando = false;
          this.formularioAberto = false;

          this.mensagemSucesso =
            'Produto cadastrado com sucesso.';

          this.carregarProdutos();
        },
        error: erro => {
          this.salvando = false;

          this.mensagemErro =
            this.obterMensagemErro(
              erro,
              'Não foi possível cadastrar o produto.'
            );
        }
      });
  }

  excluir(produto: Produto): void {
    const confirmou = confirm(
      `Deseja realmente excluir o produto "${produto.descricao}"?`
    );

    if (!confirmou) {
      return;
    }

    this.mensagemErro = '';
    this.mensagemSucesso = '';

    this.produtoServico.excluir(produto.id).subscribe({
      next: () => {
        this.mensagemSucesso =
          'Produto excluído com sucesso.';

        this.carregarProdutos();
      },
      error: erro => {
        this.mensagemErro =
          this.obterMensagemErro(
            erro,
            'Não foi possível excluir o produto.'
          );
      }
    });
  }

  private obterMensagemErro(
    erro: any,
    mensagemPadrao: string
  ): string {

    if (erro?.error?.mensagem) {
      return erro.error.mensagem;
    }

    if (typeof erro?.error === 'string') {
      return erro.error;
    }

    return mensagemPadrao;
  }
}