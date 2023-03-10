import CSS from 'csstype';

export const DContainer: CSS.Properties = {
  borderRadius: '5px',
  backgroundColor: '#333333',
  color: 'white',
};

export const LContainer: CSS.Properties = {
  borderRadius: '5px',
};

export function container(theme: string): CSS.Properties {
  switch (theme) {
    default:
    case 'light':
      return LContainer;
    case 'dark':
      return DContainer;
  }
}

export const DButton: CSS.Properties = {
  backgroundColor: '#404040',
  color: 'white',
  borderRadius: '5px',
};

export const LButton: CSS.Properties = {
  borderRadius: '5px',
};

export function button(theme: string): CSS.Properties {
  switch (theme) {
    default:
    case 'light':
      return LButton;
    case 'dark':
      return DButton;
  }
}

export const BlockButton: CSS.Properties = {
  backgroundColor: '#404040',
  color: 'white',
};

export function blockbutton(theme: string): CSS.Properties {
  switch (theme) {
    default:
    case 'light':
      return {};
    case 'dark':
      return BlockButton;
  }
}

export const Selector: CSS.Properties = {
  backgroundColor: '#333333',
  color: 'white',
};

export function selector(theme: string) {
  switch (theme) {
    default:
    case 'light':
      return {};
    case 'dark':
      return Selector;
  }
}

export function textStyle(theme: string): string {
  switch (theme) {
    default:
    case 'light':
      return 'black';
    case 'dark':
      return 'white';
  }
}

export function boxStyle(theme: string): string {
  switch (theme) {
    default:
    case 'light':
      return 'white';
    case 'dark':
      return '#404040';
  }
}

export function highlight(theme: string): string {
  switch (theme) {
    default:
    case 'light':
      return 'lightblue';
    case 'dark':
      return 'darkblue';
  }
}

export function hslswitch(theme: string): number {
  switch (theme) {
    default:
    case 'light':
      return 50;
    case 'dark':
      return 25;
  }
}
