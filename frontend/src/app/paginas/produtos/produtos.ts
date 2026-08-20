import { Component, OnInit, inject } from '@angular/core';
import { ProdutoServico } from '../../servicos/produto-servico';
import { Produto } from '../../modelos/produto';

@Component({
  selector: 'app-produtos',
  imports: [],
  templateUrl: './produtos.html',
  styleUrl: './produtos.css'
})
export class Produtos implements OnInit {

  private readonly produtoServico = inject(ProdutoServico);

  produtos: Produto[] = [];

  carregando = true;
  mensagemErro = '';

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
        console.error('Erro ao carregar produtos:', erro);

        this.mensagemErro =
          'Não foi possível carregar os produtos.';

        this.carregando = false;
      }
    });
  }
}