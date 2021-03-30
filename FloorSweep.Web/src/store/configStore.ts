import type { Configuration } from '../models/configuration';
import Store from './store';

export default class ConfigStore {static create() { return new Store<Configuration>('Configuration'); }}
