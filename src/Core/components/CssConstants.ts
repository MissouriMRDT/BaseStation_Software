import CSS from 'csstype';
import { getCurrentTheme } from './DarkMode';

export function Container(): CSS.Properties {
  let container: CSS.Properties;
  if (getCurrentTheme() === 'dark') {
    container = {
      borderRadius: '5px',
      backgroundColor: '#333333',
      color: 'white',
    };
  } else {
    container = {
      borderRadius: '5px',
    };
  }
  return container;
}

export function Button(): CSS.Properties {
  let button: CSS.Properties;
  if (getCurrentTheme() === 'dark') {
    button = {
      backgroundColor: '#404040',
      color: 'white',
      borderRadius: '5px',
    };
  } else {
    button = {
      borderRadius: '5px',
    };
  }
  return button;
}
export function BlockButton(): CSS.Properties {
  let button: CSS.Properties;
  if (getCurrentTheme() === 'dark') {
    button = {
      backgroundColor: '#404040',
      color: 'white',
    };
  } else {
    button = {};
  }
  return button;
}
export function Selector(): CSS.Properties {
  let selector: CSS.Properties;
  if (getCurrentTheme() === 'dark') {
    selector = {
      backgroundColor: '#404040',
      color: 'white',
    };
  } else {
    selector = {};
  }
  return selector;
}
// export default {
//   darkContainer,
//   lightContainer,
//   darkButton,
//   lightButton,
// };
