import { Component } from '@angular/core';
import { Header } from './layout/header/header';
import { Footer } from './layout/footer/footer';
import { TransactionList } from './features/transactions/transaction-list/transaction-list';

@Component({
  selector: 'app-root',
  imports: [Header, Footer, TransactionList],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {}