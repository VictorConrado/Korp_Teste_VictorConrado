import { ItemNotaFiscal } from './item-nota-fiscal';

export interface NotaFiscal {
  id: number;
  numero: string;
  dataEmissao: string;
  status: number;
  valorTotal: number;
  itens: ItemNotaFiscal[];
}