import type { TokenData } from '../models/tokenData';
import Store from './store';

export default class TokenStore {static create() { return new Store<TokenData>('TokenData'); }}
