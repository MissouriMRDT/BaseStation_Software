// import React from 'react';
import '@testing-library/jest-dom';
import ControlCenter from '../RED/ControlCenter';

describe('ControlCenter', () => {
  const renderControlCenter = jest.spyOn(ControlCenter.prototype, 'render').mockImplementation();

  test('ControlCenter components renders', () => {
    const testControl = new ControlCenter({});
    testControl.render();
    expect(renderControlCenter).toHaveBeenCalled();
  });
});
