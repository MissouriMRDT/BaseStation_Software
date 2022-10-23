// import React from 'react';
import '@testing-library/jest-dom';
import Timer from '../RED/components/Timer';

const renderTimer = jest.spyOn(Timer.prototype, 'render').mockImplementation();

it('Timer components renders', () => {
  const testTimer = new Timer({ timer: undefined });
  testTimer.render();
  expect(renderTimer).toHaveBeenCalled();
});
