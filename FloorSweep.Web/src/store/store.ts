// eslint-disable-next-line import/no-extraneous-dependencies
import { writable } from 'svelte/store';

export default class Store<T> {
key:string;

constructor(key:string) {
  this.key = key;
}

get(): T {
  let item = null as T;
  if ((typeof window !== 'undefined')) {
    item = JSON.parse(sessionStorage.getItem(this.key));
  }
  return item;
}

await() {
  return new Promise<T>((resolve, reject) => {
    const item = this.get();

    if (item) {
      resolve(item);
    }
    let unsub = () => {};
    unsub = writable(this.get()).subscribe((c) => {
      if (c) {
        unsub();
        resolve(c);
      }
    }, reject);
  });
}

set(item: T) {
  if ((typeof window !== 'undefined')) {
    sessionStorage.setItem(this.key, JSON.stringify(item));
  }
  writable(this.get()).set(item);
}
}
