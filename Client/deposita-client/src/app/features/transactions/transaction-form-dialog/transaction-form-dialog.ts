import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { TransactionApiService } from '../../../core/services/transaction-api.service';
import { Transaction, TransactionRequest } from '../../../core/models/transaction.model';

@Component({
  selector: 'app-transaction-form-dialog',
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule
  ],
  templateUrl: './transaction-form-dialog.html',
  styleUrl: './transaction-form-dialog.scss'
})
export class TransactionFormDialog implements OnInit {
  private fb = inject(FormBuilder);
  private api = inject(TransactionApiService);
  private dialogRef = inject<MatDialogRef<TransactionFormDialog, TransactionRequest>>(MatDialogRef);

  // null = Add mode, a transaction = Update mode
  data = inject<Transaction | null>(MAT_DIALOG_DATA);
  isEdit = !!this.data;

  // Dropdown values loaded from the API
  types = signal<string[]>([]);

  form = this.fb.nonNullable.group({
    transactionType: [this.data?.transactionType ?? '', Validators.required],
    amount: [this.data?.amount ?? 0, [Validators.required, Validators.min(0.01)]],
    description: [this.data?.description ?? '', [Validators.required, Validators.maxLength(255)]]
  });

  ngOnInit(): void {
    this.api.getTypes().subscribe(types => this.types.set(types));
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();   // shows the red error messages
      return;
    }
    this.dialogRef.close(this.form.getRawValue());
  }
}