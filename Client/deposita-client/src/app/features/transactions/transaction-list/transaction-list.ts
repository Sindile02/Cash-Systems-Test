import { Component, OnInit, inject, signal } from '@angular/core';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { TransactionApiService } from '../../../core/services/transaction-api.service';
import { Transaction, TransactionRequest } from '../../../core/models/transaction.model';
import { TransactionFormDialog } from '../transaction-form-dialog/transaction-form-dialog';

@Component({
  selector: 'app-transaction-list',
  imports: [
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatProgressBarModule,
    DatePipe,
    CurrencyPipe
  ],
  templateUrl: './transaction-list.html',
  styleUrl: './transaction-list.scss'
})
export class TransactionList implements OnInit {
  private api = inject(TransactionApiService);
  private dialog = inject(MatDialog);
  private snackBar = inject(MatSnackBar);

  // State (signals refresh the UI automatically when they change)
  transactions = signal<Transaction[]>([]);
  selected = signal<Transaction | null>(null);
  loading = signal(false);

  displayedColumns = ['transactionId', 'dateCreated', 'transactionType', 'description', 'amount'];

  ngOnInit(): void {
    this.load();
  }

  /** READ: fetch all transactions from the API */
  load(): void {
    this.loading.set(true);
    this.api.getAll().subscribe({
      next: data => {
        this.transactions.set(data);
        this.loading.set(false);
      },
      error: err => {
        this.loading.set(false);
        this.showError(err, 'Could not load transactions');
      }
    });
  }

  /** Select a row, or deselect it if it's already selected */
  select(row: Transaction): void {
    const isSame = this.selected()?.transactionId === row.transactionId;
    this.selected.set(isSame ? null : row);
  }

  /** CREATE */
  openAdd(): void {
    this.dialog.open(TransactionFormDialog, { data: null })
      .afterClosed()
      .subscribe((request?: TransactionRequest) => {
        if (!request) return; // user clicked Cancel

        this.api.create(request).subscribe({
          next: () => {
            this.notify('Transaction added');
            this.load();
          },
          error: err => this.showError(err, 'Failed to add transaction')
        });
      });
  }

  /** UPDATE */
  openUpdate(): void {
    const current = this.selected();
    if (!current) return;

    this.dialog.open(TransactionFormDialog, { data: current })
      .afterClosed()
      .subscribe((request?: TransactionRequest) => {
        if (!request) return;

        this.api.update(current.transactionId, request).subscribe({
          next: () => {
            this.notify('Transaction updated');
            this.selected.set(null);
            this.load();
          },
          error: err => this.showError(err, 'Failed to update transaction')
        });
      });
  }

  /** DELETE */
  delete(): void {
    const current = this.selected();
    if (!current) return;
    if (!confirm(`Delete transaction #${current.transactionId}?`)) return;

    this.api.delete(current.transactionId).subscribe({
      next: () => {
        this.notify('Transaction deleted');
        this.selected.set(null);
        this.load();
      },
      error: err => this.showError(err, 'Failed to delete transaction')
    });
  }

  /** Reads the ProblemDetails JSON from the API's GlobalExceptionHandler */
  private showError(err: HttpErrorResponse, fallback: string): void {
    const validationErrors = err.error?.errors
      ? Object.values(err.error.errors as Record<string, string[]>).flat()
      : [];

    const message = validationErrors[0] ?? err.error?.detail ?? err.error?.title ?? fallback;
    this.notify(message);
  }

  private notify(message: string): void {
    this.snackBar.open(message, 'Close', { duration: 3000 });
  }
}