export interface Transaction {
  transactionId: number;
  amount: number;
  description: string;
  transactionType: string;
  dateCreated: string;
}

export interface TransactionRequest {
  amount: number;
  description: string;
  transactionType: string;
}