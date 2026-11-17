import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Transaction, TransactionRequest } from '../models/transaction.model';

@Injectable({ providedIn: 'root' })
export class TransactionApiService {
  private http = inject(HttpClient);
  private baseUrl = `${environment.apiUrl}/transactions`;

  /** GET api/transactions */
  getAll() {
    return this.http.get<Transaction[]>(this.baseUrl);
  }

  /** GET api/transactions/types — for the dropdown */
  getTypes() {
    return this.http.get<string[]>(`${this.baseUrl}/types`);
  }

  /** POST api/transactions — returns the new id */
  create(request: TransactionRequest) {
    return this.http.post<number>(this.baseUrl, request);
  }

  /** PUT api/transactions/{id} */
  update(id: number, request: TransactionRequest) {
    return this.http.put<void>(`${this.baseUrl}/${id}`, request);
  }

  /** DELETE api/transactions/{id} */
  delete(id: number) {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}